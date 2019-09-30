using CheckCheque.Enums;
using CheckCheque.Models;
using ServerInvoice = UIVP.Protocol.Core.Entity.Invoice;

namespace CheckCheque.Core.Dtos
{
    internal static class InvoiceDto
    {
        internal static ServerInvoice Convert(Invoice value)
        {
            var invoice = value as Invoice;
            if (invoice == null)
            {
                return null;
            }

            var serverInvoice = new ServerInvoice();
            serverInvoice.Amount = invoice.Amount;
            serverInvoice.BankAccountNumber = invoice.BankAccountNumber;
            serverInvoice.KvkNumber = invoice.KvkNumber;
            serverInvoice.IssuerAddress = invoice.IssuerAddress;

            return serverInvoice;
        }

        internal static Invoice ConvertBack(ServerInvoice value)
        {
            var serverInvoice = value as ServerInvoice;
            if (serverInvoice == null)
            {
                return null;
            }

            var invoice = new Invoice(InvoiceReason.Unknown, "New server invoice");
            invoice.Amount = serverInvoice.Amount;
            invoice.BankAccountNumber = serverInvoice.BankAccountNumber;
            invoice.KvkNumber = serverInvoice.KvkNumber;
            invoice.IssuerAddress = serverInvoice.IssuerAddress;

            return invoice;
        }
    }
}
