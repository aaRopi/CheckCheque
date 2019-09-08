﻿using System;
using System.Windows.Input;
using CheckCheque.Enums;
using CheckCheque.Models;
using FreshMvvm;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ConceptInvoice
{
    public class InvoiceSelectedViewModel : FreshBasePageModel
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

        private string _invoiceName;
        public string InvoiceName
        {
            get => _invoiceName;
            private set
            {
                if (_invoiceName != value)
                {
                    _invoiceName = value;
                    Invoice.Name = _invoiceName;
                    RaisePropertyChanged(nameof(InvoiceName));
                }
            }
        }

        public ICommand CheckInvoiceNameCommand => new Command<string>(async (string newInvoiceName) =>
        {
            if (string.IsNullOrEmpty(newInvoiceName))
            {
                await CoreMethods.DisplayAlert("Error", "Invoice name cannot be empty", "Ok");
            }
        });

        public ICommand InvoiceVerifyOrSignAndSendCommand => new Command(async () =>
        {
            if (Invoice.Reason == InvoiceReason.SignAndSend)
            {
                await CoreMethods.DisplayAlert("SignAndSend", "Signing and sending", "Ok");
            }

            if (Invoice.Reason == InvoiceReason.Verify)
            {
                await CoreMethods.DisplayAlert("Verify", "Verifying", "Ok");
            }
        });

        public override void Init(object initData)
        {
            base.Init(initData);

            var invoice = initData as Invoice;
            if (invoice != null)
            {
                Invoice = invoice;
                InvoiceName = Invoice.Name;
            }
            else
            {
                throw new ArgumentException($"{nameof(initData)} cannot be null! This viewmodel will be useless then!");
            }

            if (!(Invoice.Reason == InvoiceReason.SignAndSend || Invoice.Reason == InvoiceReason.Verify))
            {
                throw new ArgumentException($"{nameof(initData)} is of the wrong type! The viewmodel should not have this Invoice object!");
            }

            if (string.IsNullOrEmpty(Invoice.FileName))
            {
                throw new ArgumentException($"{nameof(initData)} does not have the file path set! The viewmodel should not have this Invoice object!");
            }
        }
    }
}