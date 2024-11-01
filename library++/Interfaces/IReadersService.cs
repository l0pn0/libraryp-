using Microsoft.AspNetCore.Mvc;
using Library.Requests;

namespace library_.Interfaces
{
    public interface IReadersService
    {
        Task<IActionResult> GetReaders(int page, int pageSize);
        Task<IActionResult> GetReadersF(DateTime? registrationDate);
        Task<IActionResult> CreateNewReaders([FromQuery] CreateNewReader newReaders);
        Task<IActionResult> UpdateReaders(int id, [FromQuery] CreateNewReader updateReaders);
        Task<IActionResult> DeleteAutors(int id);
        Task<IActionResult> SearchBooks(string query);
    }
}
