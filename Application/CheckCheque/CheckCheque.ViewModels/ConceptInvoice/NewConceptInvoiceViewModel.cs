using System;
using System.Windows.Input;
using CheckCheque.Enums;
using CheckCheque.Models;
using FreshMvvm;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ConceptInvoice
{
    public class NewConceptInvoiceViewModel : FreshBasePageModel
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
                    RaisePropertyChanged(nameof(Invoice));
                }
            }
        }

        public string InvoiceName => Invoice.Name;

        public ICommand SetInvoiceReasonOnConceptCommand => new Command<InvoiceReason>(async (InvoiceReason reason) =>
        {
            switch (reason)
            {
                case InvoiceReason.SignAndSend:
                    Invoice.Reason = InvoiceReason.SignAndSend;
                    break;
                case InvoiceReason.Verify:
                    Invoice.Reason = InvoiceReason.Verify;
                    break;
                default:
                    throw new ArgumentException("InvoiceReason has to be either SignAndSend or Verify");
            }

            await CoreMethods.PushPageModel<AddDigitalInvoiceViewModel>(Invoice);
        });


        public NewConceptInvoiceViewModel()
        {
            Invoice = new Invoice(InvoiceReason.Concept, "New concept invoice");
        }
    }
}
