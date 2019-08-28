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

        public bool ShowSuccess
        {
            get { return (bool)GetValue(ShowSuccessProperty); }
            set { SetValue(ShowSuccessProperty, value); }
        }

        public DialogView()
        {
            InitializeComponent();
        }
    }
}