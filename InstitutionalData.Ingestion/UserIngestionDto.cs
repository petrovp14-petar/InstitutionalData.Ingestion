using System.ComponentModel.DataAnnotations;

namespace InstitutionalData.Ingestion.Models;

public class UserIngestionDto
{
    [Required]
    [StringLength(9, MinimumLength = 9)]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "System ID must be exactly 9 digits.")]
    public string SystemId { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
