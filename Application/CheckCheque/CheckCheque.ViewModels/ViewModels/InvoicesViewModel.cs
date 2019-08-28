using System.Collections.ObjectModel;
using CheckCheque.Enums;
using CheckCheque.ViewModels.ViewModels;
using Xamarin.Forms;

namespace CheckCheque.ViewModels
{
    public class InvoicesViewModel : BindableObject
    {
        private ObservableCollection<InvoiceViewModel> invoiceViewModels;

        public ObservableCollection<InvoiceViewModel> InvoiceViewModels
        {
            get
            {
                return this.invoiceViewModels;
            }
            set
            {
                if (this.invoiceViewModels == value)
                {
                    return;
                }

                this.invoiceViewModels = value;
                this.OnPropertyChanged();
            }
        }

        public InvoicesViewModel()
        {
            this.InvoiceViewModels = new ObservableCollection<InvoiceViewModel>();
        }
    }
}
