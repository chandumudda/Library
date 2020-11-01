using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserUI.Infrastructure.Services;
using UserUI.Models;

namespace UserUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        public IActionResult Index()
        {
            var books = _bookService.ListBooksAsync();
            return View(books.Result);
        }

        public ActionResult Review(string id)
        {
            var review = new BookReview()
            {
                BookId = id
            };
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(string bookId, IFormCollection collection)
        {
            try
            {
                var review = new AddReviewResource
                {
                    ReviewComment = collection["ReviewComment"].ToString()
                };
                var isReviewAdded = _bookService.AddBookReviewAsync(bookId, review).Result;

                if (isReviewAdded)
                    return RedirectToAction(nameof(Index));
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(string id)
        {
            var book = _bookService.GetBookByIdAsync(id).Result;
            return View(book);
        }
    }
}
