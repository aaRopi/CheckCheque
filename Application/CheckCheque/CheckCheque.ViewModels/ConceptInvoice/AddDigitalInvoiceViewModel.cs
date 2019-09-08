using System;
using CheckCheque.Models;
using FreshMvvm;

namespace CheckCheque.ViewModels.ConceptInvoice
{
    public class AddDigitalInvoiceViewModel: FreshBasePageModel
    {
        private Invoice _invoice;

        public Invoice Invoice
        {
            get => _invoice;
            private set
            {
                if (_invoice != value)
                {
                    _invoice = value;
                    RaisePropertyChanged(nameof(Invoice));
                }
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            var invoice = initData as Invoice;
            if (invoice != null)
            {
                Invoice = invoice;
            }
            else
            {
                throw new ArgumentException($"{nameof(initData)} cannot be null! This viewmodel will be useless then!");
            }
        }


    }
}
