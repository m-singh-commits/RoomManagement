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
        try
        {
            var room = _context.Database.SqlQuery<Room>(@$"
                SELECT * FROM Rooms
                WHERE Id = {id}
            ").Single();
            return Ok(room);
        }
        catch (Exception)
        {
            return NotFound($"Could not find room #{id}.");
        }
    }

    [HttpGet("Rooms")]
    public IActionResult GetRooms()
    {
            var rooms = _context.Database.SqlQuery<Room>(@$"
                SELECT * FROM Rooms
            ");
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult NewRoom(Room room)
    {
            room.Id = 0;
            _context.Add(room);
            _context.SaveChanges();

            return Ok($"Successfully inserted Room.");
    }

    [HttpPut]
    public IActionResult UpdateRoom(Room userRoom)
    {
        try
        {
            var oldRoom = _context.Rooms.Single(e => e.Id == userRoom.Id);

            oldRoom.Name = userRoom.Name;
            oldRoom.Code = userRoom.Code;
            oldRoom.Capacity = userRoom.Capacity;
            oldRoom.SupervisingUser = userRoom.SupervisingUser;
            oldRoom.Options = userRoom.Options;

            _context.SaveChanges();

            return Ok($"Successfully updated room.");
        }
        catch (Exception)
        {
            return NotFound($"Could not update room.");
        } 
    }

    [HttpDelete]
    public IActionResult DeleteRoom(int id)
    {
        try
        {
            var room = _context.Database.ExecuteSql(@$"
                DELETE FROM Rooms
                WHERE Id = {id}
            ");
            return Ok($"Sucessfully deleted room #{id}.");
        }
        catch (Exception)
        {
            return NotFound($"Could not find room #{id}.");
        }
    }
}
