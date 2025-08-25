using System.ComponentModel.DataAnnotations;

namespace OktaCapstone.Api.Models;

public class User
{
  public int Id { get; set; }

  [Required]
  [MaxLength(200)]
  public string ExternalId { get; set; } = null!;

  [Required]
  [MaxLength(320)]
  public string Email { get; set; } = null!;

  public int RoleId { get; set; }

  [Required]
  public Role Role { get; set; } = null!;
}