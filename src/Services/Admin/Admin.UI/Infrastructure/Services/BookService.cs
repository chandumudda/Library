
using Admin.UI.Models;
using LibraryCore.HttpClients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.UI.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly ILibraryClient _libraryClient;
        private readonly IOptions<ServiceUrls> _serviceUrl;
        private readonly Dictionary<string, string> _headers;

        public BookService(ILibraryClient libraryClient, IOptions<List<User>> authUsers, IOptions<ServiceUrls> serviceUrl)
        {
            _libraryClient = libraryClient;
            _serviceUrl = serviceUrl;
            var user = authUsers.Value.First();
            var coreUser = new LibraryCore.Models.User
            {
                ClaimName = user.ClaimName,
                Password =  user.Password
            };
            var accessToken = _libraryClient.GenerateAccessToken(coreUser);
            _headers = new Dictionary<string, string>
            {
                { "Authorization", accessToken }
            };
        }

        public async Task<IList<Book>> ListBooksAsync()
        {
            var booksResult = await _libraryClient.ExternalGetApiResultAsync(_serviceUrl.Value.ListBooks, _headers);
            return booksResult.StatusCode == StatusCodes.Status200OK ? JsonConvert.DeserializeObject<List<Book>>(booksResult.StatusMessage) : new List<Book>();
        }

        public async Task<bool> AddBookAsync(Book book)
        {
            var result = await _libraryClient
                                        .ExternalPostApiResultAsync(_serviceUrl.Value.PostBook, 
                                            JsonConvert.SerializeObject(book), 
                                                    _headers);
            return result.StatusCode == StatusCodes.Status201Created;
        }

        public async Task<Book> GetBookByIdAsync(string id)
        {
            var booksResult = await _libraryClient.ExternalGetApiResultAsync(string.Format(_serviceUrl.Value.GetBookById, id), _headers);
            return booksResult.StatusCode == StatusCodes.Status200OK ? JsonConvert.DeserializeObject<Book>(booksResult.StatusMessage) : new Book();
        }

        public async Task<bool> UpdateBookAsync(string id, UpdateBookResource book)
        {
            var result = await _libraryClient
                .ExternalPatchApiResultAsync(string.Format(_serviceUrl.Value.UpdateBook, id),
                    JsonConvert.SerializeObject(book),
                    _headers);
            return result.StatusCode == StatusCodes.Status204NoContent;
        }

        public async Task<bool> DeleteBookAsync(string id)
        {
            var result = await _libraryClient
                .ExternalDeleteApiResultAsync(string.Format(_serviceUrl.Value.UpdateBook, id),
                    _headers);
            return result.StatusCode == StatusCodes.Status204NoContent;
        }
    }
}
