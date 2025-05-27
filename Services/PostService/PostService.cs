using System;
using BloggingPlatfromAPI.Data;
using BloggingPlatfromAPI.DTOs;
using BloggingPlatfromAPI.DTOs.PostDTOs;
using BloggingPlatfromAPI.Helpers;
using BloggingPlatfromAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatfromAPI.Services.PostService;

public class PostService : IPostService
{
    private readonly BlogDbContext _context;
    public PostService(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<PostReadDTO> CreatePost(PostCreateDTO postCreateDTO)
    {
        var category = await _context.categories
        .FirstOrDefaultAsync(c => c.Name == postCreateDTO.Category);
        if (category == null)
        {
            category = new Category { Name = postCreateDTO.Category };
            _context.categories.Add(category);
        }
        var post = new Post
        {
            Title = postCreateDTO.Title,
            Content = postCreateDTO.Content,
            CategoryId = category.Id,
            Category = category
        };
        if (postCreateDTO.Tags != null)
        {
            foreach (var tagName in postCreateDTO.Tags)
            {
                var tag = await _context.tags
                    .FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.tags.Add(tag);
                }
                post.Tags.Add(new PostTags { Post = post, Tag = tag });
            }
        }
        _context.posts.Add(post);
        await _context.SaveChangesAsync();
        return new PostReadDTO
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = category.Name,
            Tags = post.Tags.Select(pt => pt.Tag.Name).ToList()
        };
    }

    public async Task<bool> DeletePost(int id)
    {
        var post = await _context.posts.FindAsync(id);
        if (post == null) return false;
         _context.posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<PageResult<PostReadDTO>> GetAllPosts([FromQuery] string? term, [FromQuery] int pageNumber, [FromQuery] int PageSize)
    {
        var query = _context.posts
        .Include(category => category.Category)
        .Include(postTag => postTag.Tags)
        .ThenInclude(postTag => postTag.Tag)
        .AsQueryable();

        if (!string.IsNullOrEmpty(term))
        {
            term = term.ToLower();
            query = query.Where(post => post.Title.ToLower().Contains(term) || post.Content.ToLower().Contains(term) || post.Category.Name.ToLower().Contains(term));
        }
        var totalItems = await query.CountAsync();
        var posts = await query
        .Skip((pageNumber - 1) * PageSize)
        .Take(PageSize)
        .ToListAsync();

        var postDtos = posts.Select(post => new PostReadDTO
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = post.Category?.Name,
            Tags = post.Tags.Select(tag => tag.Tag.Name).ToList()
        });

        return new PageResult<PostReadDTO>
        {
            Items = postDtos.ToList(),
            TotalCount = totalItems,
            PageNumber = pageNumber,
            PageSize = PageSize
        };
    }

    public async Task<PostReadDTO> GetPostById(int id)
    {
        var post = await _context.posts
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .ThenInclude(pt => pt.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null) return null;

        return new PostReadDTO
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = post.Category?.Name,
            Tags = post.Tags.Select(pt => pt.Tag.Name).ToList()
        };

    }


    public async Task<PostReadDTO> UpdatePost(int id, PostUpdateDto postDto)
    {
        var post = await _context.posts
        .Include(c => c.Category)
        .Include(pt => pt.Tags)
        .ThenInclude(t => t.Tag)
        .FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            return null;
        }
        if (postDto.Title != null)
        {
            post.Title = postDto.Title;
        }
        if (postDto.Content != null)
        {
            post.Content = postDto.Content;
        }
        if (postDto.Category != null)
        {
            var category = await _context.categories
                .FirstOrDefaultAsync(c => c.Name == postDto.Category);
            if (category == null)
            {
                category = new Category { Name = postDto.Category };
                _context.categories.Add(category);
            }
            post.CategoryId = category.Id;
            post.Category = category;
        }
        post.UpdatedAt = postDto.UpdatedAt;
        post.Tags.Clear();
        foreach (var tag in postDto.Tags)
        {
            var existingTag = await _context.tags
                .FirstOrDefaultAsync(t => t.Name == tag);
            if (existingTag == null)
            {
                existingTag = new Tag { Name = tag };
                _context.tags.Add(existingTag);
            }
            post.Tags.Add(new PostTags { Post = post, Tag = existingTag });
        }
        await _context.SaveChangesAsync();

        return await GetPostById(post.Id);
    }
}
