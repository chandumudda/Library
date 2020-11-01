
using System.Collections.Generic;
using System.Threading.Tasks;
using Admin.UI.Models;

namespace Admin.UI.Infrastructure.Services
{
    public interface IBookService
    {
        Task<IList<Book>> ListBooksAsync();
        Task<bool> AddBookAsync(Book book);
        Task<Book> GetBookByIdAsync(string id);
        Task<bool> UpdateBookAsync(string id, UpdateBookResource book);
        Task<bool> DeleteBookAsync(string id);
        Task<IList<BookReview>> ListBookReviewsById(string id);
    }
}
