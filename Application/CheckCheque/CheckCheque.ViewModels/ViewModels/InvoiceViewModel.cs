using System;
using System.Windows.Input;
using CheckCheque.Enums;
using CheckCheque.Models;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        private Invoice _invoice;

        public Invoice Invoice
        {
            get => _invoice;
            private set
            {
                if (_invoice != value)
                {
                    _invoice = value;
                    OnPropertyChanged();
                }
            }
        }

        public string InvoiceName => Invoice.Name;

        public ICommand SetInvoiceReasonOnConceptCommand => new Command<InvoiceReason>((InvoiceReason reason) =>
        {
            switch (reason)
            {
                case InvoiceReason.SignAndSend:
                    Invoice.Reason = InvoiceReason.SignAndSend;
                    InvoiceReasonSet?.Invoke(this, InvoiceReason.SignAndSend);
                    break;
                case InvoiceReason.Verify:
                    Invoice.Reason = InvoiceReason.Verify;
                    InvoiceReasonSet?.Invoke(this, InvoiceReason.Verify);
                    break;
                default:
                    throw new ArgumentException("InvoiceReason has to be either SignAndSend or Verify");
            }
        });

        public event EventHandler<InvoiceReason> InvoiceReasonSet;

        public InvoiceViewModel(INavigation navigation) : this(navigation, InvoiceReason.Concept, "fake invoice 3000")
        {

        }

        public InvoiceViewModel(INavigation navigation, InvoiceReason invoiceReason, string invoiceName) : base(navigation)
        {
            Invoice = new Invoice(invoiceReason, invoiceName);
        }
    }
}
