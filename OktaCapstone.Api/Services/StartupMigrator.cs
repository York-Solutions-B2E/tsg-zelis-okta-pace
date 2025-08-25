using Microsoft.EntityFrameworkCore;
using OktaCapstone.Api.Data;

namespace OktaCapstone.Api.Services;

public class StartupMigrator(IServiceProvider sp, IHostEnvironment env) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken ct)
  {
    if (!env.IsDevelopment()) return;
    using var scope = sp.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync(ct);
  }
}