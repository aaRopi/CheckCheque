using System;
using System.Collections.Generic;
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
using ServerInvoice = UIVP.Protocol.Core.Entity.Invoice;
using ServerInvoiceRepository = UIVP.Protocol.Core.Repository.InvoiceRepository;

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
            var cngKey = CngKey.Import(Convert.FromBase64String(ServicesConfig.PrivateKeyPayload), CngKeyBlobFormat.EccFullPrivateBlob);

            // TODO check with Sebastian if we need to register the company as well..
            //await PublicKeyRepository.RegisterCompanyPublicKeyAsync(ServicesConfig.CompanyKvkNumber, cngKey);

            // TODO wrap in try-catch
            var publishStatus = await InvoiceRepository.PublishInvoiceAsync(InvoiceDto.Convert(invoice), cngKey);

            return InvoicePublishStatusDto.Convert(publishStatus);
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

        public async Task<Invoice> ParseInvoiceDataFromFile(string filePath) => throw new NotImplementedException("");
    }
}