using Microsoft.EntityFrameworkCore;
using OktaCapstone.Api.Data;
using OktaCapstone.Api.Models;

namespace OktaCapstone.Api;

public static class DbHelpers
{
  public static async Task AddDefaultRolesAndClaims(AppDbContext db)
  {
    Claim? viewAuthClaim = await db.Claims.FirstOrDefaultAsync(c => c.Value == "Audit.ViewAuthEvents");
    if (viewAuthClaim is null)
    {
      viewAuthClaim = new Claim
      {
        Type = "Permissions",
        Value = "Audit.ViewAuthEvents",
      };
      db.Claims.Add(viewAuthClaim);
    }
    Claim? roleChangeClaim = await db.Claims.FirstOrDefaultAsync(c => c.Value == "Audit.RoleChanges");
    if (roleChangeClaim is null)
    {
      roleChangeClaim = new Claim
      {
        Type = "Permissions",
        Value = "Audit.RoleChanges",
      };
      db.Claims.Add(roleChangeClaim);
    }
    Role? basicUser = await db.Roles.FirstOrDefaultAsync(r => r.Name == "BasicUser");
    if (basicUser is null)
    {
      basicUser = new Role
      {
        Name = "BasicUser",
        Description = "A basic user with no audit permissions",
      };
      db.Roles.Add(basicUser);
    }
    Role? authObserver = await db.Roles.FirstOrDefaultAsync(r => r.Name == "AuthObserver");
    if (authObserver is null)
    {
      authObserver = new Role
      {
        Name = "AuthObserver",
        Description = "A user who can view basic auth events.",
      };
      db.Roles.Add(authObserver);
    }
    Role? securityAuditor = await db.Roles.FirstOrDefaultAsync(r => r.Name == "SecurityAuditor");
    if (securityAuditor is null)
    {
      securityAuditor = new Role
      {
        Name = "SecurityAuditor",
        Description = "A user who can view basic auth events and user role changes.",
      };
      db.Roles.Add(securityAuditor);
    }
    await db.SaveChangesAsync();

    // load navs before manipulating.
    await db.Entry(basicUser).Collection(r => r.Claims).LoadAsync();
    await db.Entry(authObserver).Collection(r => r.Claims).LoadAsync();
    await db.Entry(securityAuditor).Collection(r => r.Claims).LoadAsync();

    basicUser.Claims.Clear();

    authObserver.Claims.Clear();
    authObserver.Claims.Add(viewAuthClaim);

    securityAuditor.Claims.Clear();
    securityAuditor.Claims.Add(viewAuthClaim);
    securityAuditor.Claims.Add(roleChangeClaim);

    await db.SaveChangesAsync();
  }
}