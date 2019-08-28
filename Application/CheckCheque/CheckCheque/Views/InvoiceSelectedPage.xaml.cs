using System;
using CheckCheque.Enums;
using CheckCheque.ViewModels.ViewModels;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class InvoiceSelectedPage : ContentPage
    {
        private InvoiceViewModel InvoiceViewModel { get; set; }

        public InvoiceSelectedPage(InvoiceViewModel invoiceViewModel)
        {
            InitializeComponent();

            InvoiceViewModel = invoiceViewModel ?? throw new ArgumentNullException(nameof(invoiceViewModel));

            switch (InvoiceViewModel.Invoice.Reason)
            {
                case InvoiceReason.SignAndSend:
                    Title = "Sign and Send Invoice";
                    InvoiceButton.Text = "Sign And Send";
                    break;
                case InvoiceReason.Verify:
                    Title = "Verify Invoice";
                    InvoiceButton.Text = "Verify";
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
            switch (InvoiceViewModel.Invoice.Reason)
            {
                case InvoiceReason.SignAndSend:
                    break;
                case InvoiceReason.Verify:
                    break;
            }
        }
    }
}
