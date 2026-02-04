public interface ITradeOfferService
{

    // AI was used for this
    TradeOffer CreateTradeOffer(int RequestingUserId, CreateTradeOfferDTO dto);

    List<TradeOffer> GetIncomingOffers(int RequestingUserId);
    List<TradeOffer> GetOutgoingOffers(int RequestingUserId);
    TradeOffer? GetTradeOfferById(int RequestingUserId, int offerId);

    TradeOffer? ManageTradeOffer(int RequestingUserId, int offerId, ManageTradeOfferDTO dto);
}