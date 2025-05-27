using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BloggingPlatfromAPI.Services.PostService;
using BloggingPlatfromAPI.DTOs;
using BloggingPlatfromAPI.DTOs.PostDTOs;
namespace BloggingPlatfromAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("post")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(PostReadDTO), 201)]
        public async Task<IActionResult> CreatePost([FromBody] PostCreateDTO postCreateDTO)
        {
            if (postCreateDTO == null)
            {
                return BadRequest("Post data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPost = await _postService.CreatePost(postCreateDTO);
            return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
        }

        [HttpGet("posts")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetPosts([FromQuery] string? term, [FromQuery] int pageNumber, [FromQuery] int PageSize)
        {
            var posts = await _postService.GetAllPosts(term, pageNumber, PageSize);
            if (posts == null)
            {
                return NotFound("No posts found.");
            }
            return Ok(posts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeletePost(int id)
        {
            var isDeleted = await _postService.DeletePost(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(PostReadDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostUpdateDto postUpdateDto)
        {
            if (postUpdateDto == null)
            {
                return BadRequest("Post data is invalid.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPost = await _postService.UpdatePost(id, postUpdateDto);
            if (updatedPost == null)
            {
                return NotFound();
            }
            return Ok(updatedPost);
        }

    }
}
