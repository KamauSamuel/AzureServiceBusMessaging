namespace BiddingPortal
{
    public record CreateBidRequest(
        string issueNumber, int amount, bool Competitive
    );
}