using CheckCheque.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using XFApplication = Xamarin.Forms.Application;
using XFTabbedPage = Xamarin.Forms.TabbedPage;

namespace CheckCheque
{
    public partial class App : XFApplication
    {
        public App()
        {
            InitializeComponent();

            var tabPage = new XFTabbedPage();
            tabPage.Children.Add(
                new NavigationPage(new SendInvoicesPage())
                {
                    Title = "Send Invoices",
                    IconImageSource = "send_invoice_icon_30",
                    BarBackgroundColor = Color.GhostWhite,
                    BarTextColor = Color.Black
                });

            tabPage.Children.Add(
                new NavigationPage(new VerifyInvoicesPage())
                {
                    Title = "Verify Invoices",
                    IconImageSource = "verify_invoice_icon_30",
                    BarBackgroundColor = Color.GhostWhite,
                    BarTextColor = Color.Black
                });

            tabPage.Children.Add(
                new NavigationPage(new InvoicesPage())
                {
                    Title = "Your Invoices",
                    IconImageSource = "invoice_icon_30",
                    BarBackgroundColor = Color.GhostWhite,
                    BarTextColor = Color.Black
                });

            tabPage.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            tabPage.CurrentPage = tabPage.Children[1]; // Start with Verify Invoices Page.

            this.MainPage = tabPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
