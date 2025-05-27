using System;
using BloggingPlatfromAPI.Data;
using BloggingPlatfromAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatfromAPI.Repositories.TagRepository;

public class TagRepository : ITagRepository
{
    private readonly BlogDbContext _context;

    public TagRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Tag?> GetTagByNameAsync(string name)
    {
        return await _context.tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<Tag> CreateTagAsync(string name)
    {
        var tag = new Tag { Name = name };
        _context.tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }
}

