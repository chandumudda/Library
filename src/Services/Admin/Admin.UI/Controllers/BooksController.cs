using Admin.UI.Infrastructure.Services;
using Admin.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.UI.Controllers
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var book = new Book()
                {
                    Author = collection["Author"].ToString(),
                    Name = collection["Name"].ToString()
                };
                var isBookCreated = _bookService.AddBookAsync(book);

                if (isBookCreated.Result)
                    return RedirectToAction(nameof(Index));
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(string id)
        {
            var book = _bookService.GetBookByIdAsync(id);
            return View(book.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                var updateResource = new UpdateBookResource
                {
                    Author = collection["Author"].ToString(),
                    Name = collection["Name"].ToString()
                };
                var isBookUpdated = _bookService.UpdateBookAsync(id, updateResource);

                if (isBookUpdated.Result)
                    return RedirectToAction(nameof(Index));
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            var book = _bookService.GetBookByIdAsync(id);
            return View(book.Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                var isBookDeleted = _bookService.DeleteBookAsync(id);

                if (isBookDeleted.Result)
                    return RedirectToAction(nameof(Index));
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
