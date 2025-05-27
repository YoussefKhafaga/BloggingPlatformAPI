using System;
using BloggingPlatfromAPI.Helpers;
using BloggingPlatfromAPI.Model;

namespace BloggingPlatfromAPI.Repositories.PostRepository;

public interface IPostRepository
{
    public Task<PageResult<Post>> GetAllPostsAsync(int pageNumber, int pageSize, string? searchTerm = null);
    public Task<Post?> GetPostByIdAsync(int postId);
    public Task CreatePostAsync(Post post);
    public void UpdatePost(Post post);
    public void DeletePost(Post post);
}
