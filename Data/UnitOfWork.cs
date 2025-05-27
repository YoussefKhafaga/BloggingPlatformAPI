using System;

namespace BloggingPlatfromAPI.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlogDbContext _context;

    public UnitOfWork(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
