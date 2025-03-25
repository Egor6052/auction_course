using Microsoft.AspNetCore.Mvc;
using SothbeysKillerApi.Services;
using LoggerNamespace;

namespace SothbeysKillerApi.Controllers;

public record AuctionCreateRequest(string Title, DateTime Start, DateTime Finish);
public record AuctionUpdateRequest(DateTime Start, DateTime Finish);
public record AuctionResponse(Guid Id, string Title, DateTime Start, DateTime Finish);

public class Auction {
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime Start { get; set; }
    public DateTime Finish { get; set; }
}

[ApiController]
[Route("api/v1/[controller]")]
public class AuctionController : ControllerBase {
    private readonly IAuctionService _auctionService;
    private readonly Logger _logger;

    public AuctionController(IAuctionService auctionService, Logger logger) {
        _auctionService = auctionService;
        _logger = logger;
    }
    
    [HttpGet]
    [Route("[action]")]
    public IActionResult Past() {
        var auctions = _auctionService.GetPastAuctions();
        // Code 200
        _logger.LogError("Returning past auctions - Code 200");
        return Ok(auctions);
    }
    
    [HttpGet]
    [Route("[action]")]
    public IActionResult Active() {
        var auctions = _auctionService.GetActiveAuctions();
        // Code 200
        _logger.LogError("Returning active auctions - Code 200");
        return Ok(auctions);
    }
    
    [HttpGet]
    [Route("[action]")]
    public IActionResult Future() {
        var auctions = _auctionService.GetFutureAuctions();
        // Code 200
        _logger.LogError("Returning future auctions - Code 200");
        return Ok(auctions);
    }

    [HttpPost]
    public IActionResult Create(AuctionCreateRequest request) {
        try {
            var id = _auctionService.CreateAuction(request);
            // Code 200
            _logger.LogError($"Auction created with ID {id} - Code 200");
            return Ok(new { Id = id });
        }
        catch (ArgumentException) {
            // Code 200
            _logger.LogError("Invalid arguments for auction creation - Code 400");
            return BadRequest();
        }
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id) {
        try {
            var auction = _auctionService.GetAuctionById(id);
            // Code 200
            _logger.LogError($"Auction found with ID {id} - Code 200");
            return Ok(auction);
        }
        catch (NullReferenceException) {
            // Code 404
            _logger.LogError($"Auction with ID {id} not found - Code 404");
            return NotFound();
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] AuctionUpdateRequest request) {
        if (request == null || request.Start >= request.Finish) {
            // Code 400
            _logger.LogError($"Invalid update request for ID {id} - Code 400");
            return BadRequest();
        }

        try {
            _auctionService.UpdateAuction(id, request);
            // Code 204
            _logger.LogError($"Auction with ID {id} updated - Code 204");
            return NoContent();
        }
        catch (NullReferenceException) {
            // Code 404
            _logger.LogError($"Auction with ID {id} not found - Code 404");
            return NotFound();
        }
        catch (ArgumentException) {
            // Code 400
            _logger.LogError($"Invalid arguments for updating auction ID {id} - Code 400");
            return BadRequest();
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id) {
        try {
            _auctionService.DeleteAuction(id);
            // Code 204
            _logger.LogError($"Auction with ID {id} deleted - Code 204");
            return NoContent();
        }
        catch (NullReferenceException) {
            // Code 404
            _logger.LogError($"Auction with ID {id} not found - Code 404");
            return NotFound();
        }
        catch (ArgumentException) {
            // Code 400
            _logger.LogError($"Invalid arguments for deleting auction ID {id} - Code 400");
            return BadRequest();
        }
    }
}