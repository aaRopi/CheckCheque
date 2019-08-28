using System;
using System.Collections.Generic;
using CheckCheque.Enums;

namespace CheckCheque.Models
{
    public class Invoice 
    {
        // reason of invoice creation
        public InvoiceReason Reason { get; set; } = InvoiceReason.Unknown;

        // local identification data
        public Guid Id { get; private set; }
        public string FileName { get; set; }
        public string Name { get; set; }

        // last verification or sign&sent timestamp
        public DateTime LastVerified { get; set; } = DateTime.MinValue;
        public DateTime LastSignedAndSent { get; set; } = DateTime.MinValue;

        // server side properties
        public double Amount { get; set; }
        public string BankAccountNumber { get; set; }
        public string IssuerAddress { get; set; }
        public string KvkNumber { get; set; }

        // possibility of extension
        public Dictionary<string, object> InvoiceDataKeyValuePairs { get; set; }

        public Invoice(InvoiceReason invoiceReason, string invoiceName)
        {
            Reason = invoiceReason;
            Name = invoiceName;
            Id = new Guid();
        }
    }
}
