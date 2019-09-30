using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CheckCheque.Core.Dtos;
using CheckCheque.Core.Enums;
using CheckCheque.Core.Services.Interfaces;
using CheckCheque.Models;
using RestSharp;
using Tangle.Net.ProofOfWork;
using Tangle.Net.Repository;
using Tangle.Net.Repository.Client;
using UIVP.Protocol.Core.Iota.Repository;
using UIVP.Protocol.Core.Repository;
using UIVP.Protocol.Core.Services;
using Xamarin.Forms;
using Syncfusion.Pdf;
using ServerInvoice = UIVP.Protocol.Core.Entity.Invoice;
using ServerInvoiceRepository = UIVP.Protocol.Core.Repository.InvoiceRepository;
using Syncfusion.Pdf.Parsing;
using System.Linq;

[assembly: Dependency(typeof(CheckCheque.Core.Services.InvoiceService))]
namespace CheckCheque.Core.Services
{
    internal class InvoiceService : IInvoiceService
    {
        private ServerInvoiceRepository InvoiceRepository { get; }

        private IPublicKeyRepository PublicKeyRepository { get; }

        public InvoiceService(ServerInvoiceRepository invoiceRepository)
        {
            InvoiceRepository = invoiceRepository ?? throw new ArgumentNullException($"{nameof(invoiceRepository)} cannot be null");
        }

        public InvoiceService(IIotaRepository iotaRepository, IPublicKeyRepository publicKeyRepository)
            : this(new IotaInvoiceRepository(iotaRepository, publicKeyRepository))
        {
            PublicKeyRepository = publicKeyRepository ?? throw new ArgumentNullException($"{nameof(publicKeyRepository)} cannot be null");
        }

        public InvoiceService() : this(new RestIotaRepository(
                new FallbackIotaClient(new List<string> { ServicesConfig.IotaClientApiUri }, ServicesConfig.IotaClientTimeoutMilliseconds),
                new PoWService(new CpuPearlDiver())),
                new KvkPublicKeyRepository(new RestClient(ServicesConfig.KvkApiUri)))
        { }

        public async Task<InvoiceVerificationStatus> VerifyInvoiceAsync(Invoice invoice)
        {
            if (invoice == null)
                return InvoiceVerificationStatus.Unknown;

            var serverInvoice = InvoiceDto.Convert(invoice);
            if (serverInvoice == null)
                return InvoiceVerificationStatus.Unknown;

            // TODO wrap in try-catch
            var verificationStatus = await InvoiceRepository.VerifyInvoiceAsync(serverInvoice);

            return InvoiceVerificationStatusDto.Convert(verificationStatus);
        }

        public async Task<InvoicePublishStatus> PublishInvoiceAsync(Invoice invoice)
        {
            InvoicePublishStatus publishStatus = InvoicePublishStatus.Unknown;

            try
            {
                var cngKey = CngKey.Import(Convert.FromBase64String(ServicesConfig.PrivateKeyPayload), CngKeyBlobFormat.EccFullPrivateBlob);
                var serverPublishStatus = await InvoiceRepository.PublishInvoiceAsync(InvoiceDto.Convert(invoice), cngKey);
                publishStatus = InvoicePublishStatusDto.Convert(serverPublishStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return publishStatus;
        }

        public async Task<Invoice> ParseInvoiceDataFromImage(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException($"{nameof(filePath)} cannot be empty or null.");
            }

            var apiUri = ServicesConfig.VisionApiUri;
            var subscriptionKey = ServicesConfig.VisionApiSubscriptionKey;

            var imageInvoiceParser = new ImageInvoiceParser(apiUri, subscriptionKey);
            ServerInvoice serverInvoice = null;

            try
            {
                serverInvoice = await imageInvoiceParser.ParseInvoiceAsync(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return serverInvoice != null ? InvoiceDto.ConvertBack(serverInvoice) : null;
        }

        public async Task<Invoice> ParseInvoiceDataFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException($"{nameof(filePath)} cannot be null");
            }

            var kvkNumber = string.Empty;
            var bankAccount = string.Empty;
            var address = string.Empty;
            var amount = 0.00d;

            try
            {
                //byte[] fileBytes = File.ReadAllBytes(filePath);
                Stream file = new FileStream(path: filePath, mode: FileMode.Open);

                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(file);
                PdfLoadedPageCollection loadedPages = loadedDocument.Pages;

                TextLineCollection lineCollection = new TextLineCollection();
                loadedPages[0].ExtractText(out lineCollection);

                //Gets specific line from the collection
                foreach (var line in lineCollection.TextLine)
                {
                    foreach (var word in line.WordCollection)
                    {
                        if (word.Text.ToLower().Contains("kvk"))
                        {
                            kvkNumber = line.WordCollection[1].Text;
                        }

                        if (word.Text.ToLower().Contains("bank"))
                        {
                            line.WordCollection.Skip(1).Take(line.WordCollection.Count - 1).ToList().ForEach(w => bankAccount += w.Text);
                        }

                        if (word.Text.ToLower().Contains("address"))
                        {
                            line.WordCollection.Skip(1).Take(line.WordCollection.Count - 1).ToList().ForEach(w => address += $" {w.Text}");
                        }

                        if (word.Text.ToLower() == "total")
                        {
                            var possibleAmount = line.WordCollection.Last().Text;

                            if (possibleAmount.Contains('.'))
                            {
                                double.TryParse(possibleAmount.Replace('.', ','), out amount);
                            }
                        }

                    }
                }

                //Close the document
                loadedDocument.Close(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new Invoice { Amount = amount, BankAccountNumber = bankAccount, KvkNumber = kvkNumber, IssuerAddress = address };
        }
    }
}