
using System;
using System.Threading.Tasks;
using Book.API.Infrastructure.Services;
using Book.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Book.API.Controllers
{
    [Route("api")]
    [ApiController]
    
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("books/{id}/reviews")]
        public async Task<IActionResult> ListAsync(string id)
        {
            var books = await _reviewService.GetAllReviewsAsync(id);
            if (books.Any())
                return Ok(books);
            return NoContent();
        }

        [Authorize(Roles="User")]
        [HttpPost("books/{id}/reviews")]
        public async Task<IActionResult> PostAsync(string id, [FromBody] AddReviewResource input)
        {
            try
            {
                if (input == null)
                    return BadRequest("Invalid input!");

                await _reviewService.AddReviewAsync(id, input);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
