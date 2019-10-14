using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CheckCheque.Core.Repositories.Interfaces;
using CheckCheque.Models;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckCheque.Core.Repositories.InvoicesRepository))]
namespace CheckCheque.Core.Repositories
{
    internal class InvoicesRepository : IInvoicesRepository
    {
        private List<Invoice> Invoices { get; set; }
        private SQLiteConnection Database { get; }

        internal InvoicesRepository(string dbPath)
        {
            if (string.IsNullOrEmpty(dbPath))
            {
                throw new ArgumentNullException($"{nameof(dbPath)} cannot be empty or null.");
            }

            Database = new SQLiteConnection(dbPath);
            Database.CreateTable<Invoice>();
            Invoices = Database.Table<Invoice>().ToList();
        }

        public InvoicesRepository()
            : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "CheckChequeInvoices.db3"))
        {
        }

        public void AddOrUpdateInvoice(Invoice invoice)
        {
            GetInvoices();

            if (Invoices.Any(i => i.Id == invoice.Id))
                Database.Update(invoice);
            else
                Database.Insert(invoice);
        }

        public IList<Invoice> GetInvoices()
        {
            return Invoices = Database.Table<Invoice>().ToList();
        }

        public bool DeleteInvoice(Invoice invoice)
        {
            if (Invoices.Any(i => i.Id == invoice.Id))
                Database.Delete(invoice);
            else
                return false;

            return true;
        }
    }
}
