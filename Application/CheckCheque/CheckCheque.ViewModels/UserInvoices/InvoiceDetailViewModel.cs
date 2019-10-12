using System;
using System.Windows.Input;
using CheckCheque.Models;
using FreshMvvm;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.UserInvoices
{
    public class InvoiceDetailViewModel : FreshBasePageModel
    {
        private Invoice _invoice;
        public Invoice Invoice
        {
            get => _invoice;
            set
            {
                if (_invoice != value)
                {
                    _invoice = value;

                    RaisePropertyChanged(nameof(InvoiceName));
                    RaisePropertyChanged(nameof(InvoiceFileName));
                    RaisePropertyChanged(nameof(InvoiceNumber));
                    RaisePropertyChanged(nameof(InvoiceBAN));
                    RaisePropertyChanged(nameof(InvoiceIA));
                    RaisePropertyChanged(nameof(InvoiceKvkNumber));

                    RaisePropertyChanged(nameof(Invoice));
                }
            }
        }

        public string InvoiceName => Invoice?.Name;
        public string InvoiceReason => Invoice?.Reason.ToString();
        public string InvoiceFileName => Invoice?.FileName;

        public string InvoiceNumber => Invoice.InvoiceNumber; 
        public string InvoiceBAN => Invoice?.BankAccountNumber;
        public string InvoiceIA => Invoice?.IssuerAddress;
        public string InvoiceKvkNumber => Invoice?.KvkNumber;

        public ICommand ShowFileCommand => new Command(() =>
        {
            if (string.IsNullOrEmpty(InvoiceFileName))
            {
                return;
            }
        });

        public override void Init(object initData)
        {
            base.Init(initData);

            var invoice = initData as Invoice;
            if (invoice == null)
            {
                throw new ArgumentException($"{nameof(initData)} has to be a valid invoice");
            }

            Invoice = invoice;
        }
    }
}
