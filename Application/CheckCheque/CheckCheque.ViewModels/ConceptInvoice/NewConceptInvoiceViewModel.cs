using System;
using System.Windows.Input;
using CheckCheque.Enums;
using FreshMvvm;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ConceptInvoice
{
    public class NewConceptInvoiceViewModel : FreshBasePageModel
    {
        public ICommand SetInvoiceReasonOnConceptCommand => new Command<InvoiceReason>(async (InvoiceReason reason) =>
        {
            switch (reason)
            {
                case InvoiceReason.Concept:
                case InvoiceReason.Unknown:
                    throw new ArgumentException("InvoiceReason has to be either SignAndSend or Verify");
            }

            //await CoreMethods.PushPageModel<AddDigitalInvoiceViewModel>(reason, true);
            var addDigitalInvoicePage = FreshPageModelResolver.ResolvePageModel<AddDigitalInvoiceViewModel>(reason);
            var basicNavContainer = new FreshNavigationContainer(addDigitalInvoicePage, "AddNewInvoicePage");
            await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { addDigitalInvoicePage.GetModel() });
        });
    }
}
