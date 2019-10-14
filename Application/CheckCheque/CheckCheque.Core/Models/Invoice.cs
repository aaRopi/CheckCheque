using System;
using System.Collections.Generic;
using CheckCheque.Core.Enums;
using CheckCheque.Enums;
using SQLite;

namespace CheckCheque.Models
{
    [Table("Invoices")]
    public class Invoice 
    {
        // local identification data
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public InvoiceReason Reason { get; set; } = InvoiceReason.Unknown;
        public DigitalInvoiceFileType FileType { get; set; } = DigitalInvoiceFileType.Unknown;
        public string FileName { get; set; }
        public string Name { get; set; }

        // last verification or sign&sent timestamp
        public DateTime LastVerified { get; set; } = DateTime.MinValue;
        public DateTime LastSignedAndSent { get; set; } = DateTime.MinValue;

        // old server side properties
        public double Amount { get; set; } = -1;

        // new server side properties
        public string InvoiceNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string IssuerAddress { get; set; }
        public string KvkNumber { get; set; }

        // possibility of extension
        [Ignore]
        public Dictionary<string, object> InvoiceDataKeyValuePairs { get; set; }

        public Invoice()
        {
            Reason = InvoiceReason.Unknown;
        }

        public Invoice(InvoiceReason invoiceReason, string invoiceName)
        {
            Reason = invoiceReason;
            Name = invoiceName;
        }
    }
}
