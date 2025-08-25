using System.ComponentModel.DataAnnotations;

namespace OktaCapstone.Api.Models;

public class Claim
{
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Type { get; set; } = null!;

  [Required]
  [MaxLength(200)]
  public string Value { get; set; } = null!;

  public ICollection<Role> Roles { get; set; } = [];
}