using Xamarin.Forms;

namespace CheckCheque.CustomViews
{
    public partial class DialogView : ContentView
    {
        public static readonly BindableProperty ShowSuccessProperty = BindableProperty.Create(
            nameof(ShowSuccess),
            typeof(bool),
            typeof(DialogView),
            false);

        public static readonly BindableProperty InvoiceOperationResultProperty = BindableProperty.Create(
            nameof(InvoiceOperationResult),
            typeof(string),
            typeof(DialogView),
            string.Empty);

        public bool ShowSuccess
        {
            get { return (bool)GetValue(ShowSuccessProperty); }
            set { SetValue(ShowSuccessProperty, value); }
        }

        public string InvoiceOperationResult
        {
            get { return (string)GetValue(InvoiceOperationResultProperty); }
            set { SetValue(InvoiceOperationResultProperty, value); }
        }

        public DialogView()
        {
            InitializeComponent();
        }
    }
}