using CheckCheque.ConceptInvoice;
using CheckCheque.Extensions;
using CheckCheque.Others;
using CheckCheque.UserInvoices;
using CheckCheque.ViewModels.ConceptInvoice;
using CheckCheque.ViewModels.Others;
using CheckCheque.ViewModels.UserInvoices;
using FreshMvvm;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using XFApplication = Xamarin.Forms.Application;

namespace CheckCheque
{
    public partial class App : XFApplication
    {
        public App()
        {
            InitializeComponent();

            RegisterViewAndViewModelMappings();
            FreshPageModelResolver.PageModelMapper = new MyFreshPageModelMapper();

            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.AddTab<NewConceptInvoiceViewModel>("Send/Verify Invoices", "send_verify_icon_30");
            tabbedNavigation.AddTab<UserInvoicesViewModel>("Your Invoices", "your_invoices_icon_30");

            tabbedNavigation.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
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
            MyFreshPageModelMapper.Mappings.Add(typeof(ShowInstructionsViewModel), typeof(ShowInstructionsPage));
            MyFreshPageModelMapper.Mappings.Add(typeof(InvoiceSelectedViewModel), typeof(InvoiceSelectedPage));
            MyFreshPageModelMapper.Mappings.Add(typeof(InvoiceDetailViewModel), typeof(InvoiceDetailPage));
        }
    }
}