
using Book.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Book.API.Infrastructure.Services
{
    public interface IReviewService
    {
        Task AddReviewAsync(string bookId, AddReviewResource resource);
        Task<IList<BookReview>> GetAllReviewsAsync(string bookId);
    }
}
