using System;
using BloggingPlatfromAPI.Model;

namespace BloggingPlatfromAPI.Repositories.TagRepository;

public interface ITagRepository
{
    Task<Tag?> GetTagByNameAsync(string name);
    Task<Tag> CreateTagAsync(string name);
}

