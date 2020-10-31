
using System.Threading.Tasks;

namespace Book.API.Infrastructure.Services
{
    public interface IHistoryService
    {
        Task AddReviewAsync(string bookId);
    }
}
