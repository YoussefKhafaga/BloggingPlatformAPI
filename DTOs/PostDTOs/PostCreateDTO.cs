using System.ComponentModel.DataAnnotations;
using BloggingPlatfromAPI.Model;
namespace BloggingPlatfromAPI.DTOs;

public record class PostCreateDTO
{
    [Required, StringLength(50, MinimumLength = 2)]
    public  string  Title { get; set; } = "";
    [Required, StringLength(5000, MinimumLength = 10)]
    public string Content { get; set; } = "";
    [Required]
    public string Category { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public List<string> Tags = new List<string>();

}
