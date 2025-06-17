using Microsoft.EntityFrameworkCore;
using RoomManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<RoomManagementContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionStrings:RoomManagement"],
        o => o.UseCompatibilityLevel(120));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
