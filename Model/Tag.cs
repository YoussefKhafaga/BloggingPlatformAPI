using System;

namespace BloggingPlatfromAPI.Model;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<PostTags> Posts { get; set; } = new();
}
