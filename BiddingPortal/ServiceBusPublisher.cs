using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace BiddingPortal
{
    public class ServiceBusPublisher(ServiceBusClient serviceBusClient) : IServiceBusPublisher
    {
        private const string QueueName = "fibid";
        public async Task PublishBidCreatedEventAsync(BidCreatedEvent bidCreatedEvent)
        {
            var messagebody = JsonSerializer.Serialize(bidCreatedEvent);
            var message = new ServiceBusMessage(messagebody)
            {
                MessageId = bidCreatedEvent.bidId.ToString(),
                Subject = bidCreatedEvent.issueNumber,
                ApplicationProperties =
                {
                    {"Customer", bidCreatedEvent.bidder},
                    {"FaceValue", bidCreatedEvent.facevalue}
                }
            };
        
        await using var queueSender = serviceBusClient.CreateSender(QueueName);
        await queueSender.SendMessageAsync(message);
        }
    }
}