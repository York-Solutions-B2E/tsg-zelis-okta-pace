using Microsoft.EntityFrameworkCore;
using OktaCapstone.Api.Data;
using OktaCapstone.Api.Models;

namespace OktaCapstone.Api;

public class Query
{
  [UsePaging]
  [UseProjection]
  [UseFiltering]
  [UseSorting]
  public IQueryable<SecurityEvent> SecurityEvents([Service] AppDbContext db) =>
    db.SecurityEvents;

  public IQueryable<Role> Roles([Service] AppDbContext db) =>
    db.Roles;

  public IQueryable<Claim> Claims([Service] AppDbContext db) =>
    db.Claims;

  public IQueryable<User> Users([Service] AppDbContext db) =>
    db.Users;

  public Task<User?> UserById(int id, [Service] AppDbContext db) =>
    db.Users.FirstOrDefaultAsync(u => u.Id == id);
}