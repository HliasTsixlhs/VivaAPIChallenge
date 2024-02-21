using Microsoft.AspNetCore.Mvc;
using Viva.Geo.API.Common.Dtos.ArrayProcessing.Requests;
using Viva.Geo.API.Core.Abstractions.Services;


namespace Viva.Geo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArrayProcessingController : ControllerBase
{
    private readonly ISecondLargestNumberService _secondLargestNumberService;

    public ArrayProcessingController(ISecondLargestNumberService secondLargestNumberService)
    {
        _secondLargestNumberService = secondLargestNumberService;
    }

    [HttpPost("FindSecondLargest")]
    public IActionResult FindSecondLargest([FromBody] SecondLargestNumberRequest request)
    {
        var secondLargest = _secondLargestNumberService.FindSecondLargest(request.Numbers);
        return Ok(secondLargest);
    }
}