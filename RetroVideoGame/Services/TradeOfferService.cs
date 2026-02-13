

using Confluent.Kafka;
using RetroVideoGame.Models;

public class TradeOfferService : ITradeOfferService
{
    private readonly IUserService _userService;
    private readonly IGameService _gameService;

    private static readonly List<TradeOffer> _tradeOffers = new();
    private static int _nextId = 1;
    private readonly IProducer<Null, string> _producer;
    private readonly string _emailTopic;

    public TradeOfferService(IUserService UserService, IGameService GameService, IProducer<Null, string> Producer, IConfiguration configuration)
    {
        _userService = UserService;
        _gameService = GameService;
        _producer = Producer;
        _emailTopic = configuration["KAFKA_EMAIL_TOPIC"] ?? "emails";
    }

public TradeOffer? CreateTradeOffer(int requestingUserId, CreateTradeOfferDTO dto)
{
    var requestingUser = _userService.GetUserById(requestingUserId);
    if (requestingUser == null) return null;

    var requestedGame = _gameService.GetGameById(dto.RequestedGameId);
    if (requestedGame == null) return null;

    var offeringGame = _gameService.GetGameById(dto.OfferingGameId);
    if (offeringGame == null) return null;

    if (offeringGame.UserId != requestingUserId) return null;
    if (requestedGame.UserId == requestingUserId) return null;

    var offeredUser = _userService.GetUserById(requestedGame.UserId);
    if (offeredUser == null) return null;

    var tradeOffer = new TradeOffer
    {
        Id = _nextId++,
        RequestingUserId = requestingUserId,
        OfferedUserId = offeredUser.Id,
        RequestedGameId = requestedGame.Id,
        OfferingGameId = offeringGame.Id,
        Status = TradeOfferStatus.pending
    };

    _tradeOffers.Add(tradeOffer);

    var topic = string.IsNullOrWhiteSpace(_emailTopic) ? "emails" : _emailTopic;

    var emailToOffered = new EmailMessage
    {
        To = offeredUser.Email,
        Subject = "You Received A Trade Offer",
        Body = "You received a trade offer."
    };

    var emailToRequesting = new EmailMessage
    {
        To = requestingUser.Email,
        Subject = "Trade Offer Sent",
        Body = "Your trade offer has been sent."
    };

    try
    {
        var payload1 = System.Text.Json.JsonSerializer.Serialize(emailToOffered);
        _producer.Produce(topic, new Confluent.Kafka.Message<Confluent.Kafka.Null, string> { Value = payload1 });

        var payload2 = System.Text.Json.JsonSerializer.Serialize(emailToRequesting);
        _producer.Produce(topic, new Confluent.Kafka.Message<Confluent.Kafka.Null, string> { Value = payload2 });

        _producer.Flush(TimeSpan.FromSeconds(2));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[TradeOffer] Email publish failed: {ex.Message}");
    }
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

public TradeOffer? ManageTradeOffer(int requestingUserId, int offerId, ManageTradeOfferDTO dto)
{
    // AI was used to adjust my code
    var tradeOffer = GetTradeOfferById(requestingUserId, offerId);
    if (tradeOffer == null) return null;

    if (tradeOffer.Status != TradeOfferStatus.pending) return null;


    if (tradeOffer.OfferedUserId != requestingUserId) return null;

    if (dto.Status != TradeOfferStatus.accepted && dto.Status != TradeOfferStatus.rejected)
        return null;

    var offeredUser = _userService.GetUserById(tradeOffer.OfferedUserId);
    var requestingUser = _userService.GetUserById(tradeOffer.RequestingUserId);

    var requestedGame = _gameService.GetGameById(tradeOffer.RequestedGameId);
    var offeringGame  = _gameService.GetGameById(tradeOffer.OfferingGameId);

    if (offeredUser == null || requestingUser == null) return null;
    if (requestedGame == null || offeringGame == null) return null;

    if (dto.Status == TradeOfferStatus.accepted)
    {
        if (requestedGame.UserId != tradeOffer.OfferedUserId) return null;
        if (offeringGame.UserId != tradeOffer.RequestingUserId) return null;

        requestedGame.UserId = tradeOffer.RequestingUserId;
        offeringGame.UserId = tradeOffer.OfferedUserId;
    }

    tradeOffer.Status = dto.Status;

    var topic = string.IsNullOrWhiteSpace(_emailTopic) ? "emails" : _emailTopic;

    // AI was used to modify this email code
    try
    {
        var decisionWord = dto.Status == TradeOfferStatus.accepted ? "ACCEPTED" : "REJECTED";

        var emailToRequesting = new EmailMessage
        {
            To = requestingUser.Email,
            Subject = $"Your Trade Offer was {decisionWord}",
            Body =
                $"Your trade offer was {decisionWord} by {offeredUser.Name}.\n\n" +
                $"You offered: \"{offeringGame.Title}\"\n" +
                $"For: \"{requestedGame.Title}\"\n\n" +
                (dto.Status == TradeOfferStatus.accepted
                    ? "The games have been swapped between users."
                    : "No changes were made to game ownership.")
        };

        var emailToOffered = new EmailMessage
        {
            To = offeredUser.Email,
            Subject = $"You {decisionWord} a trade offer",
            Body =
                $"You {decisionWord} the trade offer from {requestingUser.Name}.\n\n" +
                $"They offered: \"{offeringGame.Title}\"\n" +
                $"For your: \"{requestedGame.Title}\"\n\n" +
                (dto.Status == TradeOfferStatus.accepted
                    ? "The games have been swapped between users."
                    : "No changes were made to game ownership.")
        };

        var payload1 = System.Text.Json.JsonSerializer.Serialize(emailToRequesting);
        _producer.Produce(topic, new Confluent.Kafka.Message<Confluent.Kafka.Null, string> { Value = payload1 });

        var payload2 = System.Text.Json.JsonSerializer.Serialize(emailToOffered);
        _producer.Produce(topic, new Confluent.Kafka.Message<Confluent.Kafka.Null, string> { Value = payload2 });

        _producer.Flush(TimeSpan.FromSeconds(2));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[TradeOffer] ManageTradeOffer email publish failed: {ex.Message}");
    }

    return tradeOffer;
}
}