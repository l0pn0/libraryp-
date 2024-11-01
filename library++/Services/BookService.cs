using Library.DataBaseContext;
using Library.Model;
using Library.Requests;
using library_.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Library.Requests.Find;

namespace library_.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryApiDB _context;
        public BookService(LibraryApiDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewBook([FromQuery] CreateNewBook newBook)
        {
            try
            {
                var book = new Book
                {
                    Title = newBook.Title,
                    Id_Autors = newBook.Id_Autors,
                    YearOfPublication = newBook.Year_public,
                    Genre_Id = newBook.Genre_Id,
                    Description = newBook.Description,
                    AvailableCopies = newBook.AvailableCopies
                };
                await _context.Book.AddAsync(book);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }


        public async Task<IActionResult> DeleteBook(int id_book)
        {

            if (id_book <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор книги.");
            }

            try
            {
                var book = await _context.Book.FindAsync(id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга с указанным идентификатором не найдена.");
                }

                _context.Book.Remove(book);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindById(int id_book)
        {
            if (id_book <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор книги.");
            }
            try
            {
                var book = await _context.Book.FindAsync(id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга с указанным идентификатором не найдена.");
                }

                var bookDto = new GetAllBooksId
                {
                    Id_Books = book.Id_Book,
                    Title = book.Title,
                    Author = book.Autors.FName + " " + book.Autors.LName,
                    Year_public = book.YearOfPublication,
                    Id_genre = book.Genre_Id,
                    Description = book.Description,
                    Copies = book.AvailableCopies
                };
                return new OkObjectResult(bookDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindByIdGenre(int id_genre)
        {
            if (id_genre <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор жанра.");
            }
            try
            {
                var books = await _context.Book.Where(b => b.Genre_Id == id_genre).ToListAsync();

                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным жанром не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksId
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title,
                    Author = b.Autors.FName + " " + b.Autors.LName,
                    Year_public = b.YearOfPublication,
                    Id_genre = b.Genre_Id,
                    Description = b.Description,
                    Copies = b.AvailableCopies
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> FindCopies(string Title)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return new BadRequestObjectResult("Название книги обязательно для поиска.");
            }

            try
            {
                var books = await _context.Book
                    .Where(b => b.Title.Contains(Title))
                    .Select(b => new BookAvailableCopies
                    {
                        Id_Books = b.Id_Book,
                        Title = b.Title,
                        AvailableCopies = b.AvailableCopies
                    })
                    .ToListAsync();

                if (books == null || books.Count == 0)
                {
                    return new NotFoundObjectResult("Книги с указанным названием не найдены.");
                }

                return new OkObjectResult(books);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetBooks(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BadRequestObjectResult("Параметры страницы и размера страницы должны быть положительными.");
            }

            try
            {
                var totalBooks = await _context.Book.CountAsync();
                var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

                var books = await _context.Book
                    .Include(b => b.Genres)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title,
                    Autor = b.Autors.FName + " " + b.Autors.LName,
                    Year_public = b.YearOfPublication,
                    genres = b.Genres.Title,
                    Description = b.Description,
                    Copies = b.AvailableCopies
                });

                var result = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalBooks = totalBooks,
                    Books = booksDto
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
        //public async Task<IActionResult> SearchBook(string title = null/*, string Author = null*/, string genre = null, int? yearOfPublication = null)
        //{
        //    int criteriaCount = 0;
        //    if (!string.IsNullOrEmpty(title)) criteriaCount++;
        //    //if (!string.IsNullOrEmpty(Author)) criteriaCount++;
        //    if (!string.IsNullOrEmpty(genre)) criteriaCount++;
        //    if (yearOfPublication.HasValue) criteriaCount++;

        //    if (criteriaCount != 1)
        //    {
        //        return new BadRequestObjectResult("Необходимо указать хотя бы один критерий поиска.");
        //    }

        //    try
        //    {
        //        var query = _context.Book.Include(b => b.Genres).AsQueryable();

        //        if (!string.IsNullOrEmpty(title))
        //        {
        //            query = query.Where(b => b.Title.Contains(title));
        //        }


        //        if (!string.IsNullOrEmpty(genre))
        //        {
        //            query = query.Where(b => b.Genres.Title.Contains(genre));
        //        }

        //        if (yearOfPublication.HasValue)
        //        {
        //            query = query.Where(b => b.YearOfPublication == yearOfPublication.Value);
        //        }

        //        var books = await query.ToListAsync();

        //        if (books == null || !books.Any())
        //        {
        //            return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
        //        }

        //        var booksDto = books.Select(b => new GetAllBooksName
        //        {
        //            Id_Books = b.Id_Book,
        //            Title = b.Title,
        //            Author = b.Author,
        //            Year_public = b.YearOfPublication,
        //            Id_genre = b.Genre_Id,
        //            Description = b.Description,
        //            Copies = b.AvailableCopies
        //        });

        //        return new OkObjectResult(booksDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
        //    }
        //}

        //public Task<IActionResult> SearchBooks(string title, string genre, int? yearOfPublication)
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task<IActionResult> SearchBook(IEnumerable<Book> books, string author, string genres, int? year)
        //{
        //    var query = books.AsQueryable();
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(author))
        //        {
        //            query = query.Where(b => b.Author != null && b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        //        }
        //        if (!string.IsNullOrEmpty(genres))
        //        {
        //            query = query.Where(b => b.Genres != null && b.Title.Contains(genres, StringComparison.OrdinalIgnoreCase));
        //        }
        //        if (year.HasValue)
        //        {
        //            query = query.Where(b => b.YearOfPublication == year.Value);
        //        }

        //        if (books == null || !books.Any())
        //        {
        //            return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
        //        }

        //        var booksDto = books.Select(b => new GetAllBooksName
        //        {
        //            Id_Books = b.Id_Book,
        //            Title = b.Title,
        //            Author = b.Author,
        //            Year_public = b.YearOfPublication,
        //            Id_genre = b.Genre_Id,
        //            Description = b.Description,
        //            Copies = b.AvailableCopies
        //        });

        //        return new OkObjectResult(booksDto);


        //    }

        //    catch (Exception ex)
        //    {
        //        return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
        //    }
        //    }
        //public async Task<IActionResult> SearchBooks(string title = null, string author = null, int? yearOfPublication = null)
        //{
        //    int criteriaCount = 0;
        //    if (!string.IsNullOrEmpty(title)) criteriaCount++;
        //    if (!string.IsNullOrEmpty(author)) criteriaCount++;

        //    if (yearOfPublication.HasValue) criteriaCount++;

        //    if (criteriaCount != 1)
        //    {
        //        return new BadRequestObjectResult("Необходимо указать хотя бы один критерий поиска.");
        //    }

        //    try
        //    {
        //        //var query = _context.Book.Include(b => b.Author).ThenInclude((Author)=>author).Include(b => b.Genres.Title).AsQueryable();
        //        var query = _context.Book
        //        .Include(b => b.Genres)
        //        .ThenInclude(g => g.Id_Genres.Title)
        //        .ToList();  

        //        if (!string.IsNullOrEmpty(title))
        //        {
        //            query = query.Where(b => b.Title.Contains(title));
        //        }

        //        if (!string.IsNullOrEmpty(author))
        //        {
        //            query = query.Where(b => b.Author.Contains(author));
        //        }


        //        if (yearOfPublication.HasValue)
        //        {
        //            query = query.Where(b => b.YearOfPublication == yearOfPublication.Value);
        //        }

        //        var books = await query.ToListAsync();

        //        if (books == null || !books.Any())
        //        {
        //            return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
        //        }

        //        var booksDto = books.Select(b => new GetAllBooksName
        //        {
        //            Id_Books = b.Id_Book,
        //            Title = b.Title,
        //            Author = b.Author,
        //            Id_genre = b.Genre_Id,
        //            Copies = b.AvailableCopies,
        //            Year_public = b.YearOfPublication,
        //            Description = b.Description
        //        });

        //        return new OkObjectResult(booksDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
        //    }
        //}

        public async Task<IActionResult> SearchBooks(string query)
        {
            if (query == null)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var books = await _context.Book.Include(b => b.Autors).Include(b => b.Genres)
       .Where(b => b.Title.Contains(query) || b.Autors.FName.Contains(query) || b.Autors.LName.Contains(query) || b.Genres.Title.Contains(query)
       || Convert.ToString(b.YearOfPublication).Contains(query))
       .ToListAsync();

                if (books == null || !books.Any())
                {
                    return new NotFoundObjectResult("Книги с указанным запросом не найдены.");
                }

                var booksDto = books.Select(b => new GetAllBooksName
                {
                    Id_Books = b.Id_Book,
                    Title = b.Title,
                    Autor = b.Autors.FName + " " + b.Autors.LName,
                    genres = b.Genres.Title,
                    Copies = b.AvailableCopies,
                    Year_public = b.YearOfPublication,
                    Description = b.Description
                });

                return new OkObjectResult(booksDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateBook(int id_book, [FromQuery] UpdateBooks Updbook)
        {
            throw new NotImplementedException();
        }
    }
}

