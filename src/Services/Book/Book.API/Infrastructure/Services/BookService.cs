
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Book.API.Models;
using LibraryCore.DataBaseContext;

namespace Book.API.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IDbRepository<Models.Book> _dbRepo;
        
        public BookService(IDbRepository<Models.Book> dbRepo)
        {
            _dbRepo = dbRepo;
        }

        public async Task<IList<Models.Book>> GetAllBooksAsync()
        {
            return await _dbRepo.GetAllAsync();
        }

        public async Task AddBookAsync(Models.Book book)
        {
            await _dbRepo.AddAsync(book);
        }

        public async Task<Models.Book> GetBookByIdAsync(string id)
        {
            Expression<Func<Models.Book, bool>> predicate = p => p.Id == id;
            return await _dbRepo.GetByIdAsync(predicate);
        }

        public async Task<bool> UpdateBookAsync(string id, UpdateBookResource resource)
        {
            Expression<Func<Models.Book, bool>> predicate = p => p.Id == id;
            var existingBook = await _dbRepo.GetByIdAsync(predicate);

            if(existingBook == null)
                throw new Exception("Specified book not found");

            existingBook.Author = resource.Author;
            existingBook.Name = resource.Name;

            return await _dbRepo.UpdateAsync(id, existingBook);
        }

        public async Task<bool> DeleteBookByIdAsync(string id)
        {
            Expression<Func<Models.Book, bool>> predicate = p => p.Id == id;
            var existingBook = await _dbRepo.GetByIdAsync(predicate);

            if (existingBook == null)
                throw new Exception("Specified book not found");

            return await _dbRepo.DeleteAsync(predicate);
        }
    }
}
