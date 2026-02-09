

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
    public TradeOffer? CreateTradeOffer(int RequestingUserId, CreateTradeOfferDTO dto)
    {

        var RequestingUser = _userService.GetUserById(RequestingUserId);
        if (RequestingUser == null)
        {
            return null;
        }
        
        var OfferedUser = _userService.GetUserById(_gameService.GetGameById(dto.RequestedGameId).UserId);
        if (OfferedUser == null)
        {
            return null;
        }

        var RequestedGame = _gameService.GetGameById(dto.RequestedGameId);
        if (RequestedGame == null)
        {
            return null;
        }

        var OfferedGame = _gameService.GetGameById(dto.OfferingGameId);
        if (OfferedGame == null)
        {
            return null;
        }

        if (OfferedGame.UserId != RequestingUserId)
        {
            return null;
        }

        if (RequestedGame.UserId == RequestingUserId)
        {
            return null;
        }

        var offeredUserId = RequestedGame.UserId;
        var offeredUser = _userService.GetUserById(offeredUserId);
        if (offeredUser == null)
        {
            return null;
        }

        var tradeOffer = new TradeOffer
        {
            Id = _nextId++,
            RequestingUserId = RequestingUserId,
            OfferedUserId = _gameService.GetGameById(dto.RequestedGameId).UserId,
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
        var offer = _tradeOffers.FirstOrDefault(t => t.Id == offerId);
        if (offer == null)
        {
            return null;
        }

        if (offer.RequestingUserId != RequestingUserId && offer.OfferedUserId != RequestingUserId)
        {
            return null;
        }
        return offer;
    }

    public TradeOffer? ManageTradeOffer(int RequestingUserId, int OfferId, ManageTradeOfferDTO dto)
    {
    // AI was used to adjust my code
    var tradeOffer = GetTradeOfferById(RequestingUserId, OfferId);
    if (tradeOffer == null) return null;

    if (tradeOffer.Status != TradeOfferStatus.pending) return null;

    if (tradeOffer.OfferedUserId != RequestingUserId) return null;

    if (dto.Status != TradeOfferStatus.accepted && dto.Status != TradeOfferStatus.rejected)
        return null;

    if (dto.Status == TradeOfferStatus.accepted)
    {
        var requestedGame = _gameService.GetGameById(tradeOffer.RequestedGameId);
        var offeringGame  = _gameService.GetGameById(tradeOffer.OfferingGameId);

        if (requestedGame == null || offeringGame == null) return null;

        if (requestedGame.UserId != tradeOffer.OfferedUserId) return null;
        if (offeringGame.UserId != tradeOffer.RequestingUserId) return null;

        requestedGame.UserId = tradeOffer.RequestingUserId;
        offeringGame.UserId = tradeOffer.OfferedUserId;
    }

    tradeOffer.Status = dto.Status;
    return tradeOffer;
    }
}