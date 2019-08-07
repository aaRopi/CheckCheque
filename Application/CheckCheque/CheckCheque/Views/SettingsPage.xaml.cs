using CheckCheque.ViewModels;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel ViewModel;

        public SettingsPage()
        {
            InitializeComponent();

            this.ViewModel = new SettingsViewModel();
            this.BindingContext = this.ViewModel;
        }
    }
}
