using System;
using BloggingPlatfromAPI.Data;
using BloggingPlatfromAPI.Model;
using Microsoft.EntityFrameworkCore;


namespace BloggingPlatfromAPI.Repositories.CategoryRepositoy;

public class CategoryRepository : ICategoryRepository
{
    private readonly BlogDbContext _context;
    public CategoryRepository(BlogDbContext context)
    {
        _context = context;
    }   
    public async Task<Category> GetCategoryByNameAsync(string categoryName)
    {
        var category =  await _context.categories.FirstOrDefaultAsync(c => c.Name == categoryName);
        if (category == null)
        {
            category = new Category { Name = categoryName };
            await _context.categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        return category;
    }
}
