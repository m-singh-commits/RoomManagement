using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Models;

[Table("Room")]
public class Room
{
    [Key]
    public int Id { get; set; }
}
