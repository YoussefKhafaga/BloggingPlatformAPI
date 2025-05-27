using System;
using BloggingPlatfromAPI.Data;
using BloggingPlatfromAPI.DTOs;
using BloggingPlatfromAPI.DTOs.PostDTOs;
using BloggingPlatfromAPI.Helpers;
using BloggingPlatfromAPI.Model;
using BloggingPlatfromAPI.Repositories.CategoryRepositoy;
using BloggingPlatfromAPI.Repositories.PostRepository;
using BloggingPlatfromAPI.Repositories.TagRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatfromAPI.Services.PostService;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPostRepository _postRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;
    public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository,
        ITagRepository tagRepository)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
    }


    public async Task<PostReadDTO> CreatePost(PostCreateDTO postCreateDTO)
    {
        var category = await _categoryRepository.GetCategoryByNameAsync(postCreateDTO.Category);
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
                var tag = await _tagRepository.GetTagByNameAsync(tagName);
                if (tag == null)
                {
                    tag = await _tagRepository.CreateTagAsync(tagName);
                }
                post.Tags.Add(new PostTags { Post = post, Tag = tag });
            }

        }
        await _postRepository.CreatePostAsync(post);
        await _unitOfWork.CompleteAsync();
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
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null) return false;
        _postRepository.DeletePost(post);
        await _unitOfWork.CompleteAsync();
        return true;

    }

    public async Task<PageResult<PostReadDTO>> GetAllPosts([FromQuery] string? term, [FromQuery] int pageNumber, [FromQuery] int PageSize)
    {
        var posts = await _postRepository.GetAllPostsAsync(pageNumber, PageSize, term);
        var totalItems = posts.TotalCount;
        var postDtos = posts.Items.Select(post => new PostReadDTO
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            Category = post.Category?.Name,
            Tags = post.Tags.Select(pt => pt.Tag.Name).ToList()
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
        var post = await _postRepository.GetPostByIdAsync(id);

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
        var post = await _postRepository.GetPostByIdAsync(id);
        if (post == null)
            return null;
            
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
            var category = await _categoryRepository.GetCategoryByNameAsync(postDto.Category);
            if (category == null)
            {
                category = new Category { Name = postDto.Category };
            }
            post.CategoryId = category.Id;
            post.Category = category;
        }
        post.UpdatedAt = postDto.UpdatedAt;
        post.Tags.Clear();
        foreach (var tagName in postDto.Tags)
        {
            var tag = await _tagRepository.GetTagByNameAsync(tagName);
            if (tag == null)
            {
                tag = await _tagRepository.CreateTagAsync(tagName);
            }
            post.Tags.Add(new PostTags { Post = post, Tag = tag });
        }

        _postRepository.UpdatePost(post);
        await _unitOfWork.CompleteAsync();

        return await GetPostById(post.Id);
    }
}
