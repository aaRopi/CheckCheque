using Xamarin.Forms;

namespace CheckCheque.CustomViews
{
    public partial class InvoiceDetailsView : ContentView
    {
        public static readonly BindableProperty InvoiceNameProperty = BindableProperty.Create(
            nameof(InvoiceName),
            typeof(string),
            typeof(InvoiceDetailsDialogView),
            "$invoice name");

        public static readonly BindableProperty InvoiceAmountProperty = BindableProperty.Create(
            nameof(InvoiceAmount),
            typeof(string),
            typeof(InvoiceDetailsDialogView),
            "$invoice amount");

        public static readonly BindableProperty BankAccountNumberProperty = BindableProperty.Create(
            nameof(BankAccountNumber),
            typeof(string),
            typeof(InvoiceDetailsDialogView),
            "$bank account number");

        public static readonly BindableProperty IssuerAddressProperty = BindableProperty.Create(
            nameof(IssuerAddress),
            typeof(string),
            typeof(InvoiceDetailsDialogView),
            "$issuer address");

        public static readonly BindableProperty KvkNumberProperty = BindableProperty.Create(
            nameof(KvkNumber),
            typeof(string),
            typeof(InvoiceDetailsDialogView),
            "$kvk number");

        public string InvoiceName
        {
            get { return (string)GetValue(InvoiceNameProperty); }
            set { SetValue(InvoiceNameProperty, value); }
        }

        public string InvoiceAmount
        {
            get { return (string)GetValue(InvoiceAmountProperty); }
            set { SetValue(InvoiceAmountProperty, value); }
        }

        public string BankAccountNumber
        {
            get { return (string)GetValue(BankAccountNumberProperty); }
            set { SetValue(BankAccountNumberProperty, value); }
        }

        public string IssuerAddress
        {
            get { return (string)GetValue(IssuerAddressProperty); }
            set { SetValue(IssuerAddressProperty, value); }
        }

        public string KvkNumber
        {
            get { return (string)GetValue(KvkNumberProperty); }
            set { SetValue(KvkNumberProperty, value); }
        }

        public InvoiceDetailsView()
        {
            InitializeComponent();
        }
    }
}
