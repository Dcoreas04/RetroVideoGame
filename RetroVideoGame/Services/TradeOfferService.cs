

public class TradeOfferService : ITradeOfferService
{
    private readonly IUserService _userService;
    private readonly IGameService _gameService;

    private static readonly List<TradeOffer> _tradeOffers = new();
    private static int _nextId = 1;
    public TradeOfferService(IUserService UserService, IGameService GameService)
    {
        _userService = UserService;
        _gameService = GameService;

    }
    public TradeOffer CreateTradeOffer(int RequestingUserId, CreateTradeOfferDTO dto)
    {
        if(_gameService.getGameById(dto.RequestedGameId) == null || _gameService.getGameById(dto.OfferingGameId) == null)
        {
            return null;
        }

        var tradeOffer = new TradeOffer
        {
            Id = _nextId++,
            RequestingUserId = RequestingUserId,
            OfferedUserId = _gameService.getGameById(dto.RequestedGameId).UserId,
            RequestedGameId = dto.RequestedGameId,
            OfferingGameId = dto.OfferingGameId,
            Status = TradeOfferStatus.pending
        };

        _tradeOffers.Add(tradeOffer);
        return tradeOffer;
    }

    public List<TradeOffer> GetIncomingOffers(int RequestingUserId)
    {
        return _tradeOffers.Where(t => t.OfferedUserId == RequestingUserId).ToList();
    }

    public List<TradeOffer> GetOutgoingOffers(int RequestingUserId)
    {
        return _tradeOffers.Where(t => t.RequestingUserId == RequestingUserId).ToList();
    }

    public TradeOffer? GetTradeOfferById(int RequestingUserId, int offerId)
    {
        return _tradeOffers.FirstOrDefault(t => t.Id == offerId);
    }

    public TradeOffer? ManageTradeOffer(int RequestingUserId, int offerId, ManageTradeOfferDTO dto)
    {
        var tradeOffer = GetTradeOfferById(RequestingUserId, offerId);
        if (tradeOffer == null)
        {
            return null;
        }

        tradeOffer.Status = dto.Status;
        return tradeOffer;
    }
}