using System.ComponentModel.DataAnnotations;

namespace BloggingPlatfromAPI.DTOs.PostDTOs;

public record class PostReadDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Category { get; set; }
    public List<string> Tags { get; set; } = new();
}
