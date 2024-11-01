using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.DataBaseContext;
using Library.Model;
using Library.Requests;
using library_.Interfaces;
using LessonApiBiblioteka.Requests;

namespace library_.Services
{
    public class RentalService : IRentalService
    {
        readonly LibraryApiDB _context;
        public RentalService(LibraryApiDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> GetCurrentRentals()
        {
            try
            {
                var rentals = await _context.RentalHistory
                    .Where(r => r.return_date == null)
                    .Include(r => r.Book)
                    .Include(r => r.Reader)
                    .Select(r => new
                    {
                        Title = r.Book.Title,
                        Reader = r.Reader.Name + " " + r.Reader.Surname,
                        RentalDate = r.rental_date,
                        DueDate = r.DueDate
                    })
                    .ToListAsync();

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetRentalHistoryByBook(int id_book)
        {
            try
            {
                var rentals = await _context.RentalHistory
                    .Where(r => r.book_id == id_book)
                    .Include(r => r.Book)
                    .Include(r => r.Reader)
                    .Select(r => new
                    {
                        Title = r.Book.Title,
                        Reader = r.Reader.Name+ " " + r.Reader.Surname,
                        RentalDate = r.rental_date,
                        DueDate = r.DueDate,
                        Return = r.return_date
                    })
                    .ToListAsync();

                if (rentals == null || rentals.Count == 0)
                {
                    return new NotFoundObjectResult("История аренды не найдена.");
                }

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetRentalHistoryByUser(int id_readers)
        {
            try
            {
                var rentals = await _context.RentalHistory
                    .Where(r => r.reader_id == id_readers)
                    .Include(r => r.Book)
                    .Include(r => r.Reader)
                    .Select(r => new
                    {
                        Title = r.Book.Title,
                        Reader = r.Reader.Name+ " " + r.Reader.Surname,
                        RentalDate = r.rental_date,
                        DueDate = r.DueDate,
                        ReturnDate = r.return_date
                    })
                    .ToListAsync();

                if (rentals == null || rentals.Count == 0)
                {
                    return new NotFoundObjectResult("История аренды не найдена.");
                }

                return new OkObjectResult(rentals);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> RentBook([FromQuery] RentBookRequest request)
        {
            if (request == null)
            {
                return new BadRequestObjectResult("Некорректные данные для аренды книги.");
            }

            try
            {
                var book = await _context.Book.FindAsync(request.Id_book);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга не найдена.");
                }

                if (book.AvailableCopies <= 0)
                {
                    return new BadRequestObjectResult("Нет доступных копий для аренды.");
                }
                var reader = await _context.Readers
                .FirstOrDefaultAsync(g => g.Id_Readers == request.Id_reader);
                var rental = new RentalHistory
                {
                    book_id = request.Id_book,
                    reader_id= reader.Id_Readers,
                    rental_date = DateTime.UtcNow,
                    DueDate = request.DueDate
                };
                book.AvailableCopies--;

                _context.RentalHistory.Add(rental);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Книга арендована.");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
        {
            if (request == null)
            {
                return new BadRequestObjectResult("Некорректные данные для сдачи книги.");
            }

            try
            {
                var rental = await _context.RentalHistory.FindAsync(request.RentalId);
                if (rental == null)
                {
                    return new NotFoundObjectResult("Аренда не найдена.");
                }

                var book = await _context.Book.FindAsync(rental.book_id);
                if (book == null)
                {
                    return new NotFoundObjectResult("Книга не найдена.");
                }

                rental.return_date = DateTime.UtcNow;
                book.AvailableCopies++;

                await _context.SaveChangesAsync();

                return new OkObjectResult("Книга возвращена.");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
