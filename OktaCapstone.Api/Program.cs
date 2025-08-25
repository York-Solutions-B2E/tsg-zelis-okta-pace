using Microsoft.EntityFrameworkCore;
using OktaCapstone.Api;
using OktaCapstone.Api.Data;
using OktaCapstone.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:Default");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
// builder.Services.AddHostedService<StartupMigrator>(); // Runs migrations in dev only.

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    // .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    // .RegisterDbContextFactory<AppDbContext>()
    .ModifyRequestOptions(o =>
    {
        o.IncludeExceptionDetails = builder.Environment.IsDevelopment();
    });

var app = builder.Build();

// app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    int count = await db.Users.CountAsync();
    Console.WriteLine($"GOT {count} USERS");
}

// GraphQL Endpoints & UI
    app.MapGraphQL("/graphql");

app.Run();
