
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("tradeoffers")]
public class TradeOfferController : ControllerBase
{
    private readonly ITradeOfferService _tradeOfferService;

    public TradeOfferController(ITradeOfferService tradeOfferService)
    {
        _tradeOfferService = tradeOfferService;
    }

    [HttpPost]
    public ActionResult<TradeOffer> CreateTradeOffer(int RequestingUserId, [FromBody] CreateTradeOfferDTO dto)
    {
        var created = _tradeOfferService.CreateTradeOffer(RequestingUserId, dto);
        return created is null ? NotFound() : Ok(created);
    }

    [HttpGet("incoming/{RequestingUserId:int}")]
    public ActionResult<List<TradeOffer>> GetIncomingOffers(int RequestingUserId)
    {
        var offers = _tradeOfferService.GetIncomingOffers(RequestingUserId);
        return offers is null ? NotFound() : Ok(offers);
    }

    [HttpGet("outgoing/{RequestingUserId:int}")]
    public ActionResult<List<TradeOffer>> GetOutgoingOffers(int RequestingUserId)
    {
        var offers = _tradeOfferService.GetOutgoingOffers(RequestingUserId);
        return offers is null ? NotFound() : Ok(offers);
    }

    [HttpGet("{RequestingUserId:int}/{offerId:int}")]
    public ActionResult<TradeOffer> GetTradeOfferById(int RequestingUserId, int offerId)
    {
        var offer = _tradeOfferService.GetTradeOfferById(RequestingUserId, offerId);
        return offer is null ? NotFound() : Ok(offer);
    }

    [HttpPatch("{RequestingUserId:int}/{offerId:int}")]
    public ActionResult<TradeOffer> ManageTradeOffer(int RequestingUserId, int offerId, [FromBody] ManageTradeOfferDTO dto)
    {
        var offer = _tradeOfferService.ManageTradeOffer(RequestingUserId, offerId, dto);
        return offer is null ? NotFound() : Ok(offer);
    }
}