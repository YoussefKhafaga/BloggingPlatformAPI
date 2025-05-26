using System.ComponentModel.DataAnnotations;

namespace BloggingPlatfromAPI.DTOs;

public record class PostUpdateDto
{
    [StringLength(50, MinimumLength = 2)]
    public string? Title { get; set; }
    [StringLength(5000, MinimumLength = 10)]
    public string? Content { get; set; }
    public string? Category { get; set; }
    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public  List<string> Tags { get; set; } = new List<string>();
}
