using Microsoft.EntityFrameworkCore;
using RowdyBattery.Data;

var builder = WebApplication.CreateBuilder(args);

string authorirty = builder.Configuration["Auth0:Authority"] ?? throw new ArgumentNullException("Auth0:Authority");
string audience = builder.Configuration["Auth0:Audience"] ?? throw new ArgumentNullException("Auth0:Audience");

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     options.Authority = authorirty;
     options.Audience = audience;
 });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("delete:catalog", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim("scope", "delete:catalog"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the application's DbContext. Use DefaultConnection or fallback to a local SQLite file.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=RowdyBattery.db";
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(connectionString));

// Configure authentication (JWT Bearer)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     // Minimal placeholder; configure Authority/Audience in appsettings or environment.
     options.RequireHttpsMetadata = false;
     options.SaveToken = true;
 });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
