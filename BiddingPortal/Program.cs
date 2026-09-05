using Azure.Messaging.ServiceBus;
using BiddingPortal;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string serviceBusConnectionstring = builder.Configuration.GetConnectionString("ServiceBus");
builder.Services.AddSingleton<ServiceBusClient>(sp => new ServiceBusClient(serviceBusConnectionstring));
builder.Services.AddScoped<IServiceBusPublisher, ServiceBusPublisher>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.MapOpenApi();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("api/TBills/Bid", async(CreateBidRequest bidrequest, IServiceBusPublisher servicebuspublisher) =>
{
    TBillBid newbid = new TBillBid(bidrequest.issueNumber, bidrequest.amount, null, bidrequest.Competitive, SourceofFunds.Local);
    DateTime biddatetime = DateTime.UtcNow;
    string bidder = "anonymous";
    var tbillbidcreatedevent = new BidCreatedEvent(newbid.BidId, newbid.issueNumber, newbid.FaceValue, biddatetime, bidder);
    await servicebuspublisher.PublishBidCreatedEventAsync(tbillbidcreatedevent);
    return Results.Ok(tbillbidcreatedevent.bidId);
});

app.UseHttpsRedirection();
app.Run();


