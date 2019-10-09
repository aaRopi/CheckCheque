using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CheckCheque.Core.Repositories.Interfaces;
using CheckCheque.Enums;
using CheckCheque.Models;
using CheckCheque.ViewModels.Helpers;
using FreshMvvm;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.UserInvoices
{
    public class UserInvoicesViewModel : FreshBasePageModel
    {
        private ObservableCollection<Grouping<InvoiceReason, Invoice>> _groupedInvoices;
        public ObservableCollection<Grouping<InvoiceReason, Invoice>> GroupedInvoices
        {
            get => _groupedInvoices;
            private set
            {
                if (_groupedInvoices != value)
                {
                    _groupedInvoices = value;
                    RaisePropertyChanged(nameof(GroupedInvoices));
                }
            }
        }

        private bool _refreshing;
        public bool Refreshing
        {
            get => _refreshing;
            set
            {
                if (_refreshing != value)
                {
                    _refreshing = value;
                    RaisePropertyChanged(nameof(Refreshing));
                }
            }
        }

        public ICommand RefreshInvoicesCommand => new Command(() =>
        {
            Refreshing = true;

            GroupedInvoices = new ObservableCollection<Grouping<InvoiceReason, Invoice>>(
                InvoicesRepository
                .GetInvoices()
                .GroupBy(invoice => invoice.Reason, invoice => invoice)
                .Select(grouping => new Grouping<InvoiceReason, Invoice>(grouping.Key, grouping)));

            Refreshing = false;
        });

        public ICommand ShowInvoiceDetailCommand => new Command<ItemTappedEventArgs>(async (ItemTappedEventArgs args) =>
        {
            var invoice = args.Item as Invoice;
            if (invoice == null)
            {
                return;
            }

            await CoreMethods.PushPageModel<InvoiceDetailViewModel>(invoice);
        });

        public ICommand DeleteInvoiceLocallyCommand => new Command<Invoice>(async (invoice) =>
        {
            if (invoice == null)
                return;

            var success = InvoicesRepository.DeleteInvoice(invoice);
            if (success)
                RefreshInvoicesCommand.Execute(this);
            else
                await CoreMethods.DisplayAlert("Something went wrong", "Could not delete invoice, please try again later..", "Ok");
        });

        protected IInvoicesRepository InvoicesRepository { get; private set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            InvoicesRepository = DependencyService.Get<IInvoicesRepository>();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            GroupedInvoices = new ObservableCollection<Grouping<InvoiceReason, Invoice>>(
                InvoicesRepository
                .GetInvoices()
                .GroupBy(invoice => invoice.Reason, invoice => invoice)
                .Select(grouping => new Grouping<InvoiceReason, Invoice>(grouping.Key, grouping)));
        }
    }
}
