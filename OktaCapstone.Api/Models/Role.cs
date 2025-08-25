using System.ComponentModel.DataAnnotations;

namespace OktaCapstone.Api.Models;

public class Role
{
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Name { get; set; } = null!;

  [Required]
  [MaxLength(200)]
  public string Description { get; set; } = null!;

  public ICollection<Claim> Claims { get; set; } = [];
}