using System;
using System.Collections.Generic;
using CheckCheque.Enums;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class SelectAppFlowPage : ContentPage
    {
        public SelectAppFlowPage()
        {
            InitializeComponent();
        }

        private async void OnTapped(object sender, EventArgs args)
        {
            AddInvoicesPage addInvoicesPage;

            if (sender == SignAndSendButton)
            {
                Console.WriteLine("sign and send button is tapped.");
                addInvoicesPage = new AddInvoicesPage(InvoiceReason.SignAndSend);                
            }
            else if (sender == VerifyButton)
            {
                Console.WriteLine("verify button is tapped.");
                addInvoicesPage = new AddInvoicesPage(InvoiceReason.Verify);
            }
            else
            {
                // this will crash the app!
                addInvoicesPage = new AddInvoicesPage(InvoiceReason.Unknown);
            }

            await Navigation.PushAsync(addInvoicesPage);
        }

    }
}
