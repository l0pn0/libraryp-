using Microsoft.AspNetCore.Mvc;
using library_.Requests;


namespace library_.Interfaces
{
    public interface IAuthorsService
    {
        Task<IActionResult> GetAutors();
        Task<IActionResult> CreateNewAutors([FromQuery] CreateNewAutors newAutors);

        Task<IActionResult> UpdateAutor(int id, [FromQuery] CreateNewAutors updateAutors);
        Task<IActionResult> DeleteAutor(int id);
    }
}
