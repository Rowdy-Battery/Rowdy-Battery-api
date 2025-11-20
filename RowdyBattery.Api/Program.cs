using Microsoft.EntityFrameworkCore;
using RowdyBattery.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the application's DbContext. Prefer a configured connection string,
// otherwise fall back to a local SQLite file database for development.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=RowdyBattery.db";
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
