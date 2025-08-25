using Microsoft.EntityFrameworkCore;
using OktaCapstone.Api.Data;
using OktaCapstone.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddHostedService<StartupMigrator>(); // Runs migrations in dev only.

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
