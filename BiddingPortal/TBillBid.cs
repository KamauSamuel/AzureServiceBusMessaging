namespace BiddingPortal
{

    public class TBillBid
    {
        private TBillBid()
        {
            
        }
            public TBillBid(string issuenumber, int facevalue, decimal? yield, bool competitive, SourceofFunds sof)
            {
                BidId = Guid.NewGuid();
                issueNumber = issuenumber;
                FaceValue = facevalue;
                Yield = yield;
                specificsourceoffunds = sof;
            }
            public Guid BidId { get; set; }
            public string issueNumber { get; set; }
            public int FaceValue { get; set; }
            public bool Competitive { get; set; }
            public decimal? Yield { get; set; }
            public SourceofFunds specificsourceoffunds { get; set; }

            public static TBillBid Create(string issuenumber, int facevalue, bool competitive, decimal? yield,  SourceofFunds sof)
            {
                return new TBillBid(issuenumber, facevalue, yield, competitive, sof);
            }
    }
}