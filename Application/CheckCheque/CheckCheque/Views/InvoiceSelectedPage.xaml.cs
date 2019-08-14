using System;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class InvoiceSelectedPage : ContentPage
    {
        public InvoiceSelectedPage()
        {
            InitializeComponent();
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
            if (sender == SignAndSendButton)
            {
                Console.WriteLine("sign and send button is tapped.");
                return;
            }

            if (sender == VerifyButton)
            {
                Console.WriteLine("verify button is tapped.");
                return;
            }
        }
    }
}
