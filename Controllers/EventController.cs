using RoomManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riok.Mapperly.Abstractions;
using Microsoft.AspNetCore.SignalR;
using RoomManagement.Hubs;

namespace RoomManagement.Controllers;

using System.Threading.Tasks;
using RoomManagement.DTOs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public partial class EventMapper
{
    [MapperIgnoreTarget(nameof(Event.Id))]
    [MapperIgnoreTarget(nameof(Event.CreatedBy))]
    [MapperIgnoreTarget(nameof(Event.CreatedAt))]
    public partial void EventToEvent(Event oldEvent, Event newEvent);
}

[ApiController]
[Route("/api/v1/[controller]")]
public class EventController : ControllerBase
{
    private readonly ILogger<EventController> _logger;
    private readonly RoomManagementContext _context;
    private readonly EventMapper _mapper = new();

    private readonly IHubContext<EventHub> _eventHub;
    public EventController(ILogger<EventController> logger, RoomManagementContext context, IHubContext<EventHub> eventHub)
    {
        _logger = logger;
        _context = context;
        _eventHub = eventHub;
    }

    [HttpGet]
    public IActionResult GetEvent(int id)
    {
        try
        {
            var calendarEvent = _context.Database.SqlQuery<EventUserDTO>(@$"
                SELECT Events.*, Users.Email, Users.Phone, Users.Name AS UserName
                FROM Events
                INNER JOIN Users ON Events.CreatedBy = Users.Id
                WHERE Events.Id ={id}
            ").Single();
            return Ok(calendarEvent);
        }
        catch (Exception)
        {
            return NotFound($"Could not find event #{id}.");
        }
    }

    [HttpGet("Events")]
    public IActionResult GetEvents(DateOnly start, DateOnly end)
    {
        // TODO: Get all events within a date range inclusive
        try
        {
            var calendarEvents = _context.Database.SqlQuery<EventUserDTO>(@$"
                SELECT Events.*, Users.Email, Users.Phone, Users.Name AS UserName
                FROM Events
                INNER JOIN Users ON Events.CreatedBy = Users.Id
                WHERE StartAt >= {start} AND EndAt <= CONCAT({end}, ' 23:59') 
                ");
            return Ok(calendarEvents);
        }
        catch (Exception)
        {
            return NotFound($"Could not find events.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> NewEvent(Event userEvent)
    {
        var room = _context.Rooms.Single(r => r.Id == userEvent.RoomId);

        var events = _context.Events.Where(e => e.RoomId == userEvent.RoomId && (
            e.EndAt > userEvent.StartAt && userEvent.EndAt > e.StartAt
        ) );

        if (events != null && events.Count() > 0)
        {
            return BadRequest("This event overlaps with another event.");
        }
        else if (room.Capacity < userEvent.Attendees)
        {
            return BadRequest($"{room.Id} does not have capacity for {userEvent.Attendees}");
        }
        else if (userEvent.ChairsNeeded != null && userEvent.ChairsNeeded < 0)
        {
            return BadRequest($"Chairs entered is less than 0");
        }
        else if (userEvent.TablesNeeded != null && userEvent.TablesNeeded < 0)
        {
            return BadRequest($"Tables entered is less than 0");
        }
        else if (userEvent.Attendees <= 0)
        {
            return BadRequest($"Attendees entered is less than 0");
        }
        else if (userEvent.EndAt <= userEvent.StartAt)
        {
            return BadRequest($"Invalid Start/Ent At.");
        }

        var newEvent = new Event();
        _mapper.EventToEvent(userEvent, newEvent);

        try
        {
            newEvent.Id = 0;
            newEvent.UpdatedAt = null;
            newEvent.UpdatedBy = null;
            newEvent.CreatedBy = 3;
            newEvent.CreatedAt = DateTime.Now;

            await _context.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            await _eventHub.Clients.All.SendAsync("NewEvent", @$"New event was created.");

            return Ok(newEvent);
        }
        catch (Exception)
        {
            return NotFound($"Could not create new event.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEvent(Event userEvent)
    {
        var room = _context.Rooms.Single(r => r.Id == userEvent.RoomId);

        var events = _context.Events.Where(e => e.RoomId == userEvent.RoomId && (
            e.EndAt > userEvent.StartAt && userEvent.EndAt > e.StartAt
        ) && e.Id != userEvent.Id);

        if (events != null && events.Count() > 0)
        {
            return BadRequest("This event overlaps with another event.");
        }
        else if (room.Capacity < userEvent.Attendees)
        {
            return BadRequest($"{room.Id} does not have capacity for {userEvent.Attendees}");
        }
        else if (userEvent.ChairsNeeded != null && userEvent.ChairsNeeded < 0)
        {
            return BadRequest($"Chairs entered is less than 0");
        }
        else if (userEvent.TablesNeeded != null && userEvent.TablesNeeded < 0)
        {
            return BadRequest($"Tables entered is less than 0");
        }
        else if (userEvent.Attendees <= 0)
        {
            return BadRequest($"Attendees entered is less than 0");
        }
        else if (userEvent.EndAt <= userEvent.StartAt)
        {
            return BadRequest($"Invalid Start/Ent At.");
        }

        try
        {
            var oldEvent = _context.Events.Single(e => e.Id == userEvent.Id);

            oldEvent.UpdatedAt = DateTime.Now;
            oldEvent.UpdatedBy = 3;

            oldEvent.RoomId = userEvent.RoomId;
            oldEvent.Name = userEvent.Name;
            oldEvent.Attendees = userEvent.Attendees;
            oldEvent.StartAt = userEvent.StartAt;
            oldEvent.EndAt = userEvent.EndAt;
            oldEvent.Comments = userEvent.Comments;
            oldEvent.Options = userEvent.Options;
            oldEvent.ChairsNeeded = userEvent.ChairsNeeded;
            oldEvent.TablesNeeded = userEvent.TablesNeeded;

            await _context.SaveChangesAsync();

            await _eventHub.Clients.All.SendAsync("UpdatedEvent", @$"Event #{userEvent.Id} was updated.");

            return Ok($"Successfully updated event.");
        }
        catch (Exception)
        {
            return NotFound($"Could not update event.");
        }
    }

    [HttpDelete]
    public async Task<IActionResult>  DeleteEvent(int id)
    {
        try
        {
            var calendarEvent = _context.Database.ExecuteSql(@$"
                DELETE FROM Events
                WHERE Id = {id}
            ");
            await _eventHub.Clients.All.SendAsync("DeletedEvent", @$"Event #{id} was deleted.");
            return Ok($"Sucessfully deleted event #{id}.");
        }
        catch (Exception)
        {
            return NotFound($"Could not find event #{id}.");
        }
    }
}
