using System;
using BloggingPlatfromAPI.Helpers;
using BloggingPlatfromAPI.DTOs.PostDTOs;
using BloggingPlatfromAPI.DTOs;
namespace BloggingPlatfromAPI.Services.PostService;

public interface IPostService
{
    Task<PostReadDTO> CreatePost(PostCreateDTO postDto);
    Task<PostReadDTO> GetPostById(int id);
    Task<PageResult<PostReadDTO>> GetAllPosts();
    Task<PostReadDTO> UpdatePost(int id, PostUpdateDto postDto);
    Task<bool> DeletePost(int id);
    Task<IEnumerable<PostReadDTO>> GetPostByWildCard(string wildCard);
}
