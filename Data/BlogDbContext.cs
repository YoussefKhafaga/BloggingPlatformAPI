using System;
using BloggingPlatfromAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatfromAPI.Data;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {

    }
    public DbSet<Post> posts => Set<Post>();
    public DbSet<Category> categories => Set<Category>();
    public DbSet<Tag> tags => Set<Tag>();
    public DbSet<PostTags> postTags => Set<PostTags>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure composite key for many-to-many join entity.
        modelBuilder.Entity<PostTags>()
            .HasKey(pt => new { pt.PostId, pt.TagId });

        // Configure relationships
        modelBuilder.Entity<PostTags>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.Tags)
            .HasForeignKey(pt => pt.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PostTags>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.Posts)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
        .HasOne(p => p.Category)
        .WithMany(c => c.Posts)
        .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.SetNull);
    }
}
