using Microsoft.AspNetCore.Mvc;
using LessonApiBiblioteka.Requests;
using Library.Requests;

namespace library_.Interfaces
{
    public interface IRentalService
    {
        Task<IActionResult> RentBook([FromQuery] RentBookRequest request);
        Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request);
        Task<IActionResult> GetRentalHistoryByUser(int id_readers);
        Task<IActionResult> GetCurrentRentals();
        Task<IActionResult> GetRentalHistoryByBook(int id_book);
    }
}
