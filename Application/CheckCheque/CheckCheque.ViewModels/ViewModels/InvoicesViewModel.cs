using System.Collections.Generic;
using System.Collections.ObjectModel;
using CheckCheque.Enums;
using CheckCheque.Models;
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

            this.CreateNewInvoice();
            this.CreateNewInvoice();
            this.CreateNewInvoice();
        }

        private void CreateNewInvoice()
        {
            InvoiceViewModel invoiceViewModel = new InvoiceViewModel();
            this.InvoiceViewModels.Add(invoiceViewModel);
        }
    }
}
