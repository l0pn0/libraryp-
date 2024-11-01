using Microsoft.AspNetCore.Mvc;
using Library.Requests;

namespace library_.Interfaces
{
    public interface IGenresService
    {
        Task<IActionResult> GetGenres();
        Task<IActionResult> CreateNewGenres([FromQuery] CreateNewGenre newGenres);
        Task<IActionResult> UpdateGenres(int id, [FromQuery] CreateNewGenre updateGenre);
        Task<IActionResult> DeleteGenres(int id);

    }
}
