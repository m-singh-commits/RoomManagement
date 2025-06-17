using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Models;

[Table("Event")]
public class Event
{
    [Key]
    public int Id { get; set; }
}
