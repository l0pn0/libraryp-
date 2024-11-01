using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.DataBaseContext;
using Library.Model;
using Library.Requests;
using library_.Interfaces;

namespace library_.Services
{
    public class ReadersService : IReadersService

    {
        readonly LibraryApiDB _context;
        public ReadersService(LibraryApiDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewReaders([FromQuery] CreateNewReader newReaders)
        {
            try
            {
                var readers = new Readers()
                {
                    Name = newReaders.name,
                    //? фней
                    Surname = newReaders.surname,
                    Birthday = newReaders.Birthday,
                    ContactDetails = newReaders.Contact_info,

                };
                await _context.Readers.AddAsync(readers);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteAutors(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор читателя.");
            }

            try
            {
                var readers = await _context.Readers.FindAsync(id);
                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найдена.");
                }

                _context.Readers.Remove(readers);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetReaders(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return new BadRequestObjectResult("Параметры страницы и размера страницы должны быть положительными.");
            }

            try
            {
                var totalReaders = await _context.Readers.CountAsync();
                var totalPages = (int)Math.Ceiling(totalReaders / (double)pageSize);

                var readers = await _context.Readers
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var readersDto = readers.Select(r => new GetAllReadersId
                {
                    Id_reader = r.Id_Readers,
                    name = r.Name,
                    sur_name = r.Surname,
                    Birth_year = r.Birthday,
                    Contact_Details = r.ContactDetails
                });

                var result = new
                {
                    CurrentPage = page,
                    TotalPages = totalPages,
                    TotalReaders = totalReaders,
                    Readers = readersDto
                };

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> SearchBooks(string query)
        {
            if (query == null)
            {
                return new BadRequestObjectResult("Строка обязательна для поиска.");
            }

            try
            {
                var readers = await _context.Readers
                   .Where(b => b.Name.Contains(query) || b.Surname.Contains(query))
                   .ToListAsync();

                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найден.");
                }

                var readersDto = readers.Select(b => new GetAllReadersId
                {
                    Id_reader = b.Id_Readers,
                    name = b.Name,
                    sur_name = b.Surname,
                    Birth_year = b.Birthday,
                    Contact_Details = b.ContactDetails
                });

                return new OkObjectResult(readersDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateReaders(int id, [FromQuery] CreateNewReader updateReaders)
        {
            if (id <= 0 || updateReaders == null)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления читателя.");
            }
            try
            {
                var readers = await _context.Readers.FindAsync(id);
                if (readers == null)
                {
                    return new NotFoundObjectResult("Читатель с указанным идентификатором не найден.");
                }
                readers.Name = updateReaders.name;
                readers.Surname = updateReaders.surname;
                readers.Birthday = updateReaders.Birthday;
                readers.ContactDetails = updateReaders.Contact_info;

                _context.Readers.Update(readers);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
        public async Task<IActionResult> GetReadersF([FromQuery] DateTime? registrationDate = null)
        {
            try
            {
                var query = _context.Readers.AsQueryable();

                //if (registrationDate.HasValue)
                //{
                //    query = query.Where(r => r.RegistrationDate == registrationDate.Value.Date);
                //}

                var readers = await query.ToListAsync();

                var readersDto = readers.Select(b => new GetAllReadersId
                {
                    Id_reader = b.Id_Readers,
                    name = b.Name,
                    sur_name = b.Surname,
                    Birth_year = b.Birthday,
                    Contact_Details = b.ContactDetails
                });

                return new OkObjectResult(readersDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
