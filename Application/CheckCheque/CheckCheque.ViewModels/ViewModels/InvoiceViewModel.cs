using System;
using CheckCheque.Models;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ViewModels
{
    public class InvoiceViewModel : BindableObject
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

        public InvoiceViewModel()
        {
            // hack: for demo
            Invoice = new Invoice(Enums.InvoiceReason.Concept, $"Invoice Name {new Random().Next(3, 17).ToString()}");
        }
    }
}
