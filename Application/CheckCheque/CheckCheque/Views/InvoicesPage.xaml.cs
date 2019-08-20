using System;
using System.ComponentModel;
using CheckCheque.ViewModels;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    [DesignTimeVisible(true)]
    public partial class InvoicesPage : ContentPage
    {
        private InvoicesViewModel _viewModel;

        public InvoicesViewModel ViewModel
        {
            get => _viewModel;

            set
            {
                if (_viewModel != value)
                {
                    _viewModel = value;
                    BindingContext = _viewModel;
                }
            }
        }

        public InvoicesPage()
        {
            InitializeComponent();
            this.ViewModel = new InvoicesViewModel();
        }

        private void OnClicked(object sender, EventArgs args)
        {
            if (sender == SettingsButton)
            {
                Navigation.PushAsync(new SettingsPage());
            }
        }

    }
}
