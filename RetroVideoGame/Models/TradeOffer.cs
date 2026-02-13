public class TradeOffer
{
    public int Id { get; set; }
    
    public int RequestingUserId { get; set; }
    public int OfferedUserId { get; set; }

    public int RequestedGameId { get; set; }
    public int OfferingGameId { get; set; }

    public TradeOfferStatus Status { get; set; }
}