using System;
using System.Threading.Tasks;
using Book.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Book.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book != null)
                return Ok(book);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            if (books.Any())
                return Ok(books);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Models.Book item)
        {
            try
            {
                if (item == null)
                    return BadRequest("Invalid input!");

                await _bookService.AddBookAsync(item);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> ModifyAsync(string id, [FromBody] Models.UpdateBookResource resource)
        {
            try
            {
                if (await _bookService.UpdateBookAsync(id, resource))
                    return NoContent();
                return BadRequest("Could not update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                if (await _bookService.DeleteBookByIdAsync(id))
                    return NoContent();
                return BadRequest("Could not delete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
