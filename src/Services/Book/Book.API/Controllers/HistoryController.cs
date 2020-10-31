
using System;
using System.Threading.Tasks;
using Book.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("books/{id}/completed")]
        public async Task<IActionResult> PostAsync(string id)
        {
            try
            {
                await _historyService.AddReviewAsync(id);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
