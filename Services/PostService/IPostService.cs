using System;
using BloggingPlatfromAPI.Helpers;
using BloggingPlatfromAPI.DTOs.PostDTOs;
using BloggingPlatfromAPI.DTOs;
namespace BloggingPlatfromAPI.Services.PostService;

public interface IPostService
{
    Task<PostReadDTO> CreatePost(PostCreateDTO postDto);
    Task<PostReadDTO> GetPostById(int id);
    Task<PageResult<PostReadDTO>> GetAllPosts(string term, int pageNumber, int PageSize);
    Task<PostReadDTO> UpdatePost(int id, PostUpdateDto postDto);
    Task<bool> DeletePost(int id);
}
