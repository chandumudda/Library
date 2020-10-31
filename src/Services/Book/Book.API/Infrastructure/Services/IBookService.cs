
using System.Collections.Generic;
using System.Threading.Tasks;
using Book.API.Models;

namespace Book.API.Infrastructure.Services
{
    public interface IBookService
    {
        Task<IList<Models.Book>> GetAllBooksAsync();
        Task AddBookAsync(Models.Book book);
        Task<Models.Book> GetBookByIdAsync(string id);
        Task<bool> UpdateBookAsync(string id, UpdateBookResource resource);
        Task<bool> DeleteBookByIdAsync(string id);
    }
}
