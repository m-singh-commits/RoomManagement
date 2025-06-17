using RoomManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class RoomController : ControllerBase
{
    private readonly ILogger<RoomController> _logger;
    private readonly RoomManagementContext _context;

    public RoomController(ILogger<RoomController> logger, RoomManagementContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetRoom(int id)
    {
        // TODO: Get a room by it's #
        return Ok();
    }

    [HttpGet("Rooms")]
    public IActionResult GetRooms(int id)
    {
        // TODO: Get all rooms
        return Ok();
    }

    [HttpPost]
    public IActionResult NewRoom()
    {
        // TODO: Create a new room
        return Ok();
    }

    [HttpPut]
    public IActionResult UpdateRoom()
    {
        // TODO: Update an existing room
        return Ok();
    }

    [HttpDelete]
    public IActionResult DeleteRoom()
    {
        // TODO: Delete a room
        return Ok();
    }
}
