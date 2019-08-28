using CheckCheque.Enums;
using CheckCheque.ViewModels;
using CheckCheque.ViewModels.ViewModels;

namespace CheckCheque.Views
{
    public partial class SelectAppFlowPage : ContentBasePage
    {
        private ViewModelBase _viewModel;

        public override ViewModelBase ViewModel
        {
            get => _viewModel;
            protected set
            {
                if (value != _viewModel)
                {
                    _viewModel = value;

                    BindingContext = _viewModel;
                    OnPropertyChanged();
                }
            }
        }

        public SelectAppFlowPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel = new InvoiceViewModel(Navigation, InvoiceReason.Concept, "New invoice name_1");
            (ViewModel as InvoiceViewModel).InvoiceReasonSet -= OnInvoiceReasonSetAsync;
            (ViewModel as InvoiceViewModel).InvoiceReasonSet += OnInvoiceReasonSetAsync;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (ViewModel as InvoiceViewModel).InvoiceReasonSet -= OnInvoiceReasonSetAsync;
        }

        private async void OnInvoiceReasonSetAsync(object sender, InvoiceReason e) => await Navigation.PushAsync(new AddInvoicesPage(ViewModel as InvoiceViewModel));
    }
}
