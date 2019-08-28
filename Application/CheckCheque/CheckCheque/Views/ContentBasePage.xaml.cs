using CheckCheque.ViewModels;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public abstract partial class ContentBasePage : ContentPage
    {
        public abstract ViewModelBase ViewModel { get; protected set; }

        public ContentBasePage()
        {
            InitializeComponent();
        }
    }
}
