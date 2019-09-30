using CheckCheque.Core.Enums;
using UIVP.Protocol.Core.Repository;

namespace CheckCheque.Core.Dtos
{
    internal static class InvoicePublishStatusDto
    {
        internal static InvoicePublishStatus Convert(PublishStatus value)
        {
            var status = InvoicePublishStatus.Unknown;

            switch (value)
            {
                case PublishStatus.Success:
                    status = InvoicePublishStatus.Success;
                    break;
                case PublishStatus.AlreadyPublished:
                    status = InvoicePublishStatus.AlreadyPublished;
                    break;
            }

            return status;
        }
    }
}
