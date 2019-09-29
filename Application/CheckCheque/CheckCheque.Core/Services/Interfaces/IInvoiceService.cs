using System.Threading.Tasks;
using CheckCheque.Core.Enums;
using CheckCheque.Models;

namespace CheckCheque.Core.Services.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceVerificationStatus> VerifyInvoiceAsync(Invoice invoice);

        Task<Invoice> ParseInvoiceDataFromImage(string filePath);
    }
}
