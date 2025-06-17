using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Models;

public class RoomManagementContext : DbContext
{
    public RoomManagementContext()
    {
    }

    public RoomManagementContext(DbContextOptions<RoomManagementContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Room { get; set; }

    public DbSet<Event> Events { get; set; }
}
