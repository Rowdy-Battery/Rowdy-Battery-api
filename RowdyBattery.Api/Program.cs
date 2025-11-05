using Microsoft.EntityFrameworkCore;
using RowdyBattery.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext (SQLite file in API project folder)
builder.Services.AddDbContext<StoreContext>(opt =>
    opt.UseSqlite("Data Source=Registrar.sqlite"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
