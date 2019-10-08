using System.Windows.Input;
using Xamarin.Forms;

namespace CheckCheque.CustomViews
{
    public partial class InvoiceDetailsDialogView : ContentView
    {
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

        public static readonly BindableProperty OkCommandProperty = BindableProperty.Create(
            nameof(OkCommand),
            typeof(ICommand),
            typeof(InvoiceDetailsDialogView),
            null);

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

        public ICommand OkCommand
        {
            get { return (ICommand)GetValue(OkCommandProperty); }
            set { SetValue(OkCommandProperty, value); }
        }

        public ICommand EnterManuallyCommand => new Command(() =>
        {
            InvoiceAmountEntry.IsEnabled = BankAccountNumberEntry.IsEnabled = IssuerAddressEntry.IsEnabled = KvkNumberEntry.IsEnabled = true;

            InvoiceAmountEntry.Focus();
        });

        public InvoiceDetailsDialogView()
        {
            InitializeComponent();
        }
    }
}
