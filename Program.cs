using Microsoft.EntityFrameworkCore;
using BloggingPlatfromAPI.Data;
using BloggingPlatfromAPI.Services.PostService;
using BloggingPlatfromAPI.Repositories.PostRepository;
using BloggingPlatfromAPI.Repositories.CategoryRepositoy;
using BloggingPlatfromAPI.Repositories.TagRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// Register DbContext
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("BloggingPlatformDb"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("BloggingPlatformDb"))
    )
);

// Register your services
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Enable Swagger UI and JSON
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use routing and controllers
app.UseAuthorization();
app.MapControllers();

app.Run();
