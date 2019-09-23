namespace CheckCheque.Core.Enums
{
    public enum InvoiceVerificationStatus
    {
        Unknown,

        Success,

        HashMismatch,

        MetadataUnavailable,

        KvkNumberUnavailable,

        PublicKeyNotFound,

        InvalidSignature
    }
}
