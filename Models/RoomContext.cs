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

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Event> Events { get; set; }
    
    public DbSet<User> Users { get; set; }
}
