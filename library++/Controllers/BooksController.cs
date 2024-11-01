using Library.DataBaseContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Library.Model;
using Library.Requests;
using static System.Reflection.Metadata.BlobBuilder;
using library_.Interfaces;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookService _booksService;


        public BooksController(IBookService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        [Route("getAllBooks")]
        public async Task<IActionResult> GetBooks(int page, int pageSize)
        {
            return await _booksService.GetBooks(page, pageSize);
        }

        [HttpGet("FindById/{id_book}")]
        public async Task<IActionResult> FindById(int id_book)
        {
            return await _booksService.FindById(id_book);
        }

        [HttpGet("FindByIdGenre/{id_genre}")]
        public async Task<IActionResult> FindByIdGenre(int id_genre)
        {
            return await _booksService.FindByIdGenre(id_genre);
        }

        [HttpPost]
        [Route("createNewBook")]
        public async Task<IActionResult> CreateNewBook([FromQuery] CreateNewBook newBook)
        {
            return await _booksService.CreateNewBook(newBook);
        }

        [HttpDelete("DeleteBook/{id_book}")]
        public async Task<IActionResult> DeleteBook(int id_book)
        {
            return await _booksService.DeleteBook(id_book);
        }

        [HttpPut("UpdateBook/{id_book}")]
        public async Task<IActionResult> UpdateBook(int id_book, [FromQuery] UpdateBooks Updbook)
        {
            return await _booksService.UpdateBook(id_book, Updbook);
        }

        //public async Task<IActionResult> SearchBook(string title = null,/* string Author = null,*/ string genre = null, int? yearOfPublication = null)
        //{
        //    return await _booksService.SearchBooks(title, genre, yearOfPublication);
        //}
        //[HttpGet("search")]
        //public async Task<IActionResult> SearchBook(IEnumerable<Book> books, string author, string genres, int? year)
        //{
        //    return await _booksService.SearchBook(books, author, genres, year);
        //}


        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string query)
        {
            return await _booksService.SearchBooks(query);
        }






        [HttpGet("FindCopies/{Title}")]
        public async Task<IActionResult> FindCopies(string Title)
        {
            return await _booksService.FindCopies(Title);
        }

    }

}