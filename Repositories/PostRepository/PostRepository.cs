using System;
using BloggingPlatfromAPI.Data;
using BloggingPlatfromAPI.Helpers;
using BloggingPlatfromAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatfromAPI.Repositories.PostRepository;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _context;
    public PostRepository(BlogDbContext context)
    {
        _context = context;
    }
    public async Task CreatePostAsync(Post post)
    {
        await _context.posts.AddAsync(post);
    }

    public void DeletePost(Post post)
    {
        _context.posts.Remove(post);
    }

    public async Task<PageResult<Post>> GetAllPostsAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var post = _context.posts
            .Include(post => post.Category)
            .Include(post => post.Tags)
            .ThenInclude(pt => pt.Tag) // Corrected ThenInclude usage
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            post = post.Where(p => p.Title.ToLower().Contains(searchTerm)
                                || p.Content.ToLower().Contains(searchTerm));
        }
        var totalCount = await post.CountAsync();
        var posts = await post
            .Skip((pageNumber - 1) * pageSize) // Corrected Skip calculation
            .Take(pageSize)
            .ToListAsync();

        return new PageResult<Post>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = posts
        };
    }

    public async Task<Post?> GetPostByIdAsync(int postId)
    {
        return await _context.posts
        .Include(post => post.Category)
        .Include(post => post.Tags)
        .ThenInclude(pt => pt.Tag)
        .FirstOrDefaultAsync(post => post.Id == postId);
    }

    public void UpdatePost(Post post)
    {
        _context.posts.Update(post);
    }
}
