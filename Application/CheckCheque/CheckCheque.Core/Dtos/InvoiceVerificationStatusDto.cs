using CheckCheque.Core.Enums;
using UIVP.Protocol.Core.Repository;

namespace CheckCheque.Core.Dtos
{
    internal static class InvoiceVerificationStatusDto
    {
        internal static InvoiceVerificationStatus Convert(object value)
        {
            var validStatus = value is VerificationStatus;
            if (!validStatus)
            {
                return InvoiceVerificationStatus.Unknown;
            }

            var status = InvoiceVerificationStatus.Unknown;

            switch ((VerificationStatus)value)
            {
                case VerificationStatus.Success:
                    status = InvoiceVerificationStatus.Success;
                    break;
                case VerificationStatus.HashMismatch:
                    status = InvoiceVerificationStatus.HashMismatch;
                    break;
                case VerificationStatus.MetadataUnavailable:
                    status = InvoiceVerificationStatus.MetadataUnavailable;
                    break;
                case VerificationStatus.KvkNumberUnavailable:
                    status = InvoiceVerificationStatus.KvkNumberUnavailable;
                    break;
                case VerificationStatus.PublicKeyNotFound:
                    status = InvoiceVerificationStatus.PublicKeyNotFound;
                    break;
                case VerificationStatus.InvalidSignature:
                    status = InvoiceVerificationStatus.InvalidSignature;
                    break;
            }

            return status;
        }
    }
}
