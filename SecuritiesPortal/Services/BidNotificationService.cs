using System.Text.Json;
using Azure.Messaging.ServiceBus;

namespace SecuritiesPortal.Services
{
    public class BidNotificationService(ServiceBusClient serviceBusClient, ILogger<BidNotificationService> logger, IBidCache _bidcache) : BackgroundService
    {
        private const string QueueName = "fibid";
      

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueTask = ConsumeFromQueueAsnyc(stoppingToken);
            await Task.WhenAll(queueTask);
        }
        public async Task ConsumeFromQueueAsnyc(CancellationToken cancellationToken)
        {
            await using var processor = serviceBusClient.CreateProcessor(QueueName,
            new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1
            });
            processor.ProcessMessageAsync += args => ProcessMessageAsync(args, "fibid");
            processor.ProcessErrorAsync += ProcessErrorAsync;
            await processor.StartProcessingAsync(cancellationToken);
            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);

            }catch(OperationCanceledException)
            {
                
            }
            await processor.StopProcessingAsync(cancellationToken);
        }
        private async Task ProcessMessageAsync(ProcessMessageEventArgs args, string source)
        {
            try
            {
                var messageBody = args.Message.Body.ToString();
                var bidEvent = JsonSerializer.Deserialize<BidCreatedEvent>(messageBody);
                if(bidEvent != null)
                {
                   _bidcache.AddOrUpdate(bidEvent.issueNumber, bidEvent.facevalue);
                   logger.LogInformation("{Source} message received - Bid Id: {BidId}", source, bidEvent.bidId.ToString());
                    
                    //
                }
                foreach(var bidsummary in _bidcache.GetAllItems())
                {
                    logger.LogInformation(" TBill: {bid}. The Total bids Received are: {TotalBids} at a cost of: {TotalBidAmount}", bidsummary.IssueNumber, bidsummary.TotalBidsReceived, bidsummary.TotalAmount);
                }
                await args.CompleteMessageAsync(args.Message);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error processing {Source} message", source);
                await args.AbandonMessageAsync(args.Message);
            }
        }
        private Task ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            logger.LogError(args.Exception, "Service Bus error: {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        }
    }
}