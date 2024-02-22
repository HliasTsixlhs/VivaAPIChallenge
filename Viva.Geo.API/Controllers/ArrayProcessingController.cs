using Microsoft.AspNetCore.Mvc;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Requests;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Responses;
using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;

namespace Viva.Geo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArrayProcessingController : ControllerBase
{
    private readonly IArrayProcessingService _arrayProcessingService;
    private readonly ILogger<ArrayProcessingController> _logger;
    private readonly IEventIdFactory _eventIdFactory;

    public ArrayProcessingController(
        IArrayProcessingService arrayProcessingService,
        ILogger<ArrayProcessingController> logger,
        IEventIdFactory eventIdFactory)
    {
        _arrayProcessingService = arrayProcessingService;
        _logger = logger;
        _eventIdFactory = eventIdFactory;
    }

    [HttpPost("second-largest")]
    public IActionResult FindSecondLargest([FromBody] SecondLargestNumberRequest request)
    {
        if (request.Numbers == null || request.Numbers.Count() < 2)
        {
            var eventId = _eventIdFactory.Create(VivaGeoApiEvent.ArrayProcessing);
            _logger.LogWarning(eventId, "Invalid request received. At least two numbers are required.");
            return BadRequest("At least two numbers are required.");
        }

        var secondLargest = _arrayProcessingService.FindSecondLargest(request.Numbers);
        var response = new SecondLargestNumberResponse
        {
            SecondLargestNumber = secondLargest
        };
        return Ok(response);
    }
}