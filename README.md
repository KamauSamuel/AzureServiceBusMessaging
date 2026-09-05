# Securities Bidding

A demo solution showing event-driven communication between two ASP.NET Core services using Azure Service Bus. Bidders submit Treasury Bill (T-Bill) bids through `BiddingPortal`; `SecuritiesPortal` consumes bid events asynchronously and maintains a running total per issue.

## Architecture

```mermaid
flowchart LR
    Client -->|POST api/TBills/Bid| BiddingPortal
    BiddingPortal -->|publishes BidCreatedEvent| Queue[(Azure Service Bus\nqueue: fibid)]
    Queue -->|consumes| SecuritiesPortal
    SecuritiesPortal --> BidCache[(In-memory Bid Cache)]
