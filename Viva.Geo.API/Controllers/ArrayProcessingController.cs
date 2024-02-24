using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Requests;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Responses;
using Common.Logging.Serilog.Enums;
using Common.Logging.Serilog.Factories.Abstractions;

namespace Viva.Geo.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
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
    public IActionResult FindSecondLargest([FromBody] SecondLargestIntegerRequest request)
    {
        var eventId = _eventIdFactory.Create(VivaGeoApiEvent.ArrayProcessing);
        var connectionRemotePort = HttpContext.Connection.RemotePort;

        using (_logger.BeginScope(new Dictionary<string, object> {["ConnectionRemotePort"] = connectionRemotePort}))
        {
            _logger.LogInformation(
                eventId: eventId,
                message: "Processing array to find the second largest integer. Array: {Array}",
                request.Numbers);


            var secondLargest = _arrayProcessingService.FindSecondLargest(request.Numbers);
            var response = new SecondLargestIntegerResponse
            {
                SecondLargestInteger = secondLargest
            };

            return Ok(response);
        }
    }
}