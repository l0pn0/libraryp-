using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.DataBaseContext;
using Library.Model;
using Library.Requests;
using library_.Interfaces;
using Praktos.Requests;

namespace library_.Services
{
    public class GenresService : IGenresService
    {
        readonly LibraryApiDB _context;
        public GenresService(LibraryApiDB context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateNewGenres([FromQuery] CreateNewGenre newGenres)
        {
            try
            {
                var genres = new Genres()
                {
                    Title = newGenres.Name_genre,
                };
                await _context.Genres.AddAsync(genres);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> DeleteGenres(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("Некорректный идентификатор жанра.");
            }

            try
            {
                var genres = await _context.Genres.FindAsync(id);
                if (genres == null)
                {
                    return new NotFoundObjectResult("Жанр с указанным идентификатором не найден.");
                }

                _context.Genres.Remove(genres);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> GetGenres()
        {
            try
            {
                var genres = await _context.Genres.ToListAsync();
                var genresDto = genres.Select(g => new GetAllGenres
                {
                    Id_Genres = g.Id_Genres,
                    Title = g.Title,
                });
                return new OkObjectResult(genresDto);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }

        public async Task<IActionResult> UpdateGenres(int id, [FromQuery] CreateNewGenre updateGenre)
        {
            if (id <= 0 || updateGenre == null)
            {
                return new BadRequestObjectResult("Некорректные данные для обновления жанра.");
            }
            try
            {
                var genres = await _context.Genres.FindAsync(id);
                if (genres == null)
                {
                    return new NotFoundObjectResult("Жанр с указанным идентификатором не найден.");
                }

                genres.Title = updateGenre.Name_genre;

                _context.Genres.Update(genres);
                await _context.SaveChangesAsync();


                return new NoContentResult();
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Внутренняя ошибка сервера: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
