using Library.DataBaseContext;
using Library.Model;
using Library.Requests;
using library_.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly IGenresService _genresService;
        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }
        [HttpGet]
        [Route("getAllGenres")]
        public async Task<IActionResult> GetGenres()
        {
            return await _genresService.GetGenres();
        }

        [HttpPost]
        [Route("CreateNewGenres")]
        public async Task<IActionResult> CreateNewGenres([FromQuery] CreateNewGenre newGenres)
        {
            return await _genresService.CreateNewGenres(newGenres);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenres(int id, [FromQuery] CreateNewGenre updateGenre)
        {
            return await _genresService.UpdateGenres(id, updateGenre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenres(int id)
        {
            return await _genresService.DeleteGenres(id);
        }
    }
}