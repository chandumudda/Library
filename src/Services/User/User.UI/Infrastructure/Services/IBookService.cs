using System.Collections.Generic;
using System.Threading.Tasks;
using UserUI.Models;

namespace UserUI.Infrastructure.Services
{
    public interface IBookService
    {
        Task<bool> AddBookReviewAsync(string id, AddReviewResource resource);
        Task<IList<Book>> ListBooksAsync();
        Task<Book> GetBookByIdAsync(string id);
    }
}
