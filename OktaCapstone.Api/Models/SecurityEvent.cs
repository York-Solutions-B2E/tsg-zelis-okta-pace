using System.ComponentModel.DataAnnotations;

namespace OktaCapstone.Api.Models;

public class SecurityEvent
{
  public int Id { get; set; }

  [Required]
  [MaxLength(50)]
  public string EventType { get; set; } = null!;

  public int AuthorId { get; set; }
  [Required]
  public User Author { get; set; } = null!;

  public int TargetId { get; set; }
  [Required]
  public User Target { get; set; } = null!;
  public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

  [Required]
  [MaxLength(400)]
  public string Details { get; set; } = null!;
}