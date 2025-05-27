using System;
using BloggingPlatfromAPI.Model;

namespace BloggingPlatfromAPI.Repositories.CategoryRepositoy;

public interface ICategoryRepository
{
    public Task<Category>  GetCategoryByNameAsync(string categoryName);
}
