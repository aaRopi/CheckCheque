using CheckCheque.ConceptInvoice;
using CheckCheque.Extensions;
using CheckCheque.UserInvoices;
using CheckCheque.ViewModels.ConceptInvoice;
using CheckCheque.ViewModels.UserInvoices;
using CheckCheque.Views;
using FreshMvvm;
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

            /*
            var tabPage = new XFTabbedPage();
            tabPage.SelectedTabColor = Color.FromHex("1ABC9C");
            tabPage.Children.Add(
                new NavigationPage(new SelectAppFlowPage())
                {
                    Title = "Send/Verify Invoices",
                    IconImageSource = "send_verify_icon_30",
                    BarBackgroundColor = Color.GhostWhite,
                    BarTextColor = Color.Black
                });

            tabPage.Children.Add(
                new NavigationPage(new InvoicesPage())
                {
                    Title = "Your Invoices",
                    IconImageSource = "your_invoices_icon_30",
                    BarBackgroundColor = Color.GhostWhite,
                    BarTextColor = Color.Black
                });

            tabPage.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            this.MainPage = tabPage;
            */


            RegisterViewAndViewModelMappings();
            FreshPageModelResolver.PageModelMapper = new MyFreshPageModelMapper();

            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<NewConceptInvoiceViewModel>("Send/Verify Invoices", "send_verify_icon_30");
            tabbedNavigation.AddTab<UserInvoicesViewModel>("Your Invoices", "your_invoices_icon_30");
            MainPage = tabbedNavigation;
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


        private void RegisterViewAndViewModelMappings()
        {
            MyFreshPageModelMapper.Mappings.Add(typeof(NewConceptInvoiceViewModel), typeof(NewConceptInvoicePage));
            MyFreshPageModelMapper.Mappings.Add(typeof(UserInvoicesViewModel), typeof(UserInvoicesPage));
            MyFreshPageModelMapper.Mappings.Add(typeof(AddDigitalInvoiceViewModel), typeof(AddDigitalInvoicePage));
        }
    }
}
