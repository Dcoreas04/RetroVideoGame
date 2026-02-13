using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("tradeoffers")]
public class TradeOfferController : ControllerBase
{
    private readonly ITradeOfferService _tradeOfferService;
    private readonly IUserService _userService;

    public TradeOfferController(ITradeOfferService tradeOfferService, IUserService userService)
    {
            _tradeOfferService = tradeOfferService;
            _userService = userService;
    }

    [HttpPost]
    public ActionResult<TradeOffer> CreateTradeOffer(
        [FromHeader(Name = "X-User-Id")] int? userId,
        [FromBody] CreateTradeOfferDTO dto)
    {
        if (userId == null)
            return Unauthorized("Missing X-User-Id header.");

        if (_userService.GetUserById(userId.Value) == null)
            return Unauthorized("Invalid user.");

        var created = _tradeOfferService.CreateTradeOffer(userId.Value, dto);
        return created == null ? BadRequest() : Ok(created);
    }

    [HttpGet("incoming")]
    public ActionResult<List<TradeOffer>> GetIncomingOffers(
        [FromHeader(Name = "X-User-Id")] int? userId)
    {
        if (userId == null)
            return Unauthorized();

        return Ok(_tradeOfferService.GetIncomingOffers(userId.Value));
    }

    [HttpGet("outgoing")]
    public ActionResult<List<TradeOffer>> GetOutgoingOffers(
        [FromHeader(Name = "X-User-Id")] int? userId)
    {
        if (userId == null)
            return Unauthorized();

        return Ok(_tradeOfferService.GetOutgoingOffers(userId.Value));
    }

    [HttpGet("{offerId:int}")]
    public ActionResult<TradeOffer> GetTradeOfferById(
        int offerId,
        [FromHeader(Name = "X-User-Id")] int? userId)
    {
        if (userId == null)
            return Unauthorized();

        var offer = _tradeOfferService.GetTradeOfferById(userId.Value, offerId);
        return offer == null ? NotFound() : Ok(offer);
    }

    [HttpPatch("{offerId:int}")]
    public ActionResult<TradeOffer> ManageTradeOffer(
        int offerId,
        [FromHeader(Name = "X-User-Id")] int? userId,
        [FromBody] ManageTradeOfferDTO dto)
    {
        if (userId == null)
            return Unauthorized();

        var updated = _tradeOfferService.ManageTradeOffer(userId.Value, offerId, dto);
        return updated == null ? BadRequest() : Ok(updated);
    }
}