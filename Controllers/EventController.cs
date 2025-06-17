using RoomManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class EventController : ControllerBase
{
    private readonly ILogger<EventController> _logger;
    private readonly RoomManagementContext _context;

    public EventController(ILogger<EventController> logger, RoomManagementContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetEvent(int id)
    {
        // TODO: Get an event by it's #
        return Ok();
    }

    [HttpGet("Events")]
    public IActionResult GetEvents(DateOnly start, DateOnly end)
    {
        // TODO: Get all events within a date range inclusive
        return Ok();
    }

    [HttpPost]
    public IActionResult NewEvent()
    {
        // TODO: Create a new event
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateEvent()
    {
        // TODO: Update an existing event
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteEvent()
    {
        // TODO: Delete an event
        return Ok();
    }
}
