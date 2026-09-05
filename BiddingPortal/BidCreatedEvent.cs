using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiddingPortal
{
    public record BidCreatedEvent(
        Guid bidId, string issueNumber, int facevalue, DateTime createdat, string bidder
    );
}