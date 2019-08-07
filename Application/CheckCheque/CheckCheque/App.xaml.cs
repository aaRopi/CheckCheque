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
            tabPage.Children.Add(new NavigationPage(new InvoicesPage()) { Title = "Invoices", IconImageSource = "invoice_icon_30" });
            tabPage.Children.Add(new NavigationPage(new AddInvoicesPage()) { Title = "Add Invoices", IconImageSource = "add_invoice_icon_30" });
            tabPage.Children.Add(new NavigationPage(new SettingsPage()) { Title = "Settings", IconImageSource = "settings_icon_30" });

            tabPage.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

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
