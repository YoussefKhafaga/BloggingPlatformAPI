using System;

namespace BloggingPlatfromAPI.Model;

public class PostTags
{
    public int PostId { get; set; }
    public Post Post { get; set; } = new Post();

    public int TagId { get; set; }
    public Tag Tag { get; set; } = new Tag();
}
