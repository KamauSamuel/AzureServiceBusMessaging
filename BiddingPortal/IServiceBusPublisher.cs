using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiddingPortal
{
    public interface IServiceBusPublisher
    {
        Task PublishBidCreatedEventAsync(BidCreatedEvent bidCreatedEvent);
    }
}