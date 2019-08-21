using System;
using CheckCheque.Enums;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class InvoiceSelectedPage : ContentPage
    {
        private InvoiceReason _invoiceReason = InvoiceReason.Unknown;
        private string _invoiceButtonText;

        public string InvoiceButtonText
        {
            get => _invoiceButtonText;
            set
            {
                if (_invoiceButtonText != value)
                {
                    _invoiceButtonText = value;
                    OnPropertyChanged(nameof(InvoiceButtonText));
                }
            }
        }

        public InvoiceSelectedPage(InvoiceReason invoiceReason)
        {
            InitializeComponent();

            if (invoiceReason == InvoiceReason.Unknown)
            {
                throw new ArgumentException($"{nameof(invoiceReason)} cannot be unknown.");
            }

            _invoiceReason = invoiceReason;

            Title = "Selected Invoice";
            switch (_invoiceReason)
            {
                case InvoiceReason.SignAndSend:
                    Title = InvoiceButtonText = "Sign and Send Invoice";
                    break;
                case InvoiceReason.Verify:
                    Title = InvoiceButtonText = "Verify Invoice";
                    break;
            }

        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == e.OldTextValue)
            {
                return;
            }

            InvoiceName.Text = e.NewTextValue;
        }

        private void OnTapped(object sender, EventArgs args)
        {
            switch (_invoiceReason)
            {
                case InvoiceReason.SignAndSend:
                    //await Navigation.PushModalAsync(new DialogPage());
                    break;
                case InvoiceReason.Verify:
                    break;
            }
        }
    }
}
