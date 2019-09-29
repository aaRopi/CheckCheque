using System;
using System.Collections.Generic;
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
        public ServerInvoiceRepository InvoiceRepository { get; }

        public InvoiceService(ServerInvoiceRepository invoiceRepository)
        {
            InvoiceRepository = invoiceRepository ?? throw new ArgumentNullException($"{nameof(invoiceRepository)} cannot be null");
        }

        public InvoiceService() : this(new IotaInvoiceRepository(
                new RestIotaRepository(
                new FallbackIotaClient(new List<string> { "https://nodes.devnet.thetangle.org:443" }, 5000),
                new PoWService(new CpuPearlDiver())),
                new KvkPublicKeyRepository(new RestClient("https://pactwrapper.azurewebsites.net/"))))
        {

        }

        public async Task<InvoiceVerificationStatus> VerifyInvoiceAsync(Invoice invoice)
        {
            if (invoice == null)
                return InvoiceVerificationStatus.Unknown;

            var serverInvoice = InvoiceDto.Convert(invoice);
            if (serverInvoice == null)
                return InvoiceVerificationStatus.Unknown;

            var verificationStatus = await InvoiceRepository.VerifyInvoiceAsync(serverInvoice);

            return InvoiceVerificationStatusDto.Convert(verificationStatus);
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