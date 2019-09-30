using System.Collections.Generic;
using CheckCheque.Models;

namespace CheckCheque.Core.Repositories.Interfaces
{
    public interface IInvoicesRepository
    {
        IList<Invoice> GetInvoices();

        void AddOrUpdateInvoice(Invoice invoice);
    }
}
