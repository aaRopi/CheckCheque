using System;
using System.Threading.Tasks;
using CheckCheque.Core.Dtos;
using CheckCheque.Core.Enums;
using CheckCheque.Models;
using UIVP.Protocol.Core.Repository;
using UIVP.Protocol.Core.Services;

namespace CheckCheque.Core.Services
{
    public class InvoiceService
    {
        public InvoiceRepository InvoiceRepository { get; }

        public InvoiceService(InvoiceRepository invoiceRepository)
        {
            InvoiceRepository = invoiceRepository ?? throw new ArgumentNullException($"{nameof(invoiceRepository)} cannot be null");
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

            // TODO: get from SecureStorage, but before that get from secure api
            var apiUri = "https://kvkinvoicescan.cognitiveservices.azure.com/vision/v2.0/ocr";
            var subscriptionKey = "41a0a15e78204dd08ef3ad6528595bd3";

            var imageInvoiceParser = new ImageInvoiceParser(apiUri, subscriptionKey);
            var serverInvoice = await imageInvoiceParser.ParseInvoiceAsync(filePath);

            return InvoiceDto.ConvertBack(serverInvoice);
        }
    }
}
