using Library.Model;
using Library.Requests;
using Microsoft.AspNetCore.Mvc;

namespace library_.Interfaces
{
    public interface IBookService
    {
        Task<IActionResult> CreateNewBook([FromQuery] CreateNewBook newBook);
        Task<IActionResult> DeleteBook(int id_book);
        Task<IActionResult> FindById(int id_book);
        Task<IActionResult> FindByIdGenre(int id_genre);
        Task<IActionResult> FindCopies(string Title);
        Task<IActionResult> GetBooks(int page, int pageSize);
        //Task<IActionResult> SearchBooks(string title, string genre, int? yearOfPublication);


        Task<IActionResult> SearchBooks(string query);

        Task<IActionResult> UpdateBook(int id_book, [FromQuery] UpdateBooks Updbook);
    }
}
