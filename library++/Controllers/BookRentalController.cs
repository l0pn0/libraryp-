using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Requests;
using Library.DataBaseContext;
using Library.Model;
using LessonApiBiblioteka.Requests;
using library_.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BookRentalController : ControllerBase
{
    private readonly IRentalService _rentalService;
    public BookRentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost("rent")]
    public async Task<IActionResult> RentBook([FromQuery] RentBookRequest request)
    {
        return await _rentalService.RentBook(request);
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnBook([FromQuery] ReturnBookRequest request)
    {
        return await _rentalService.ReturnBook(request);
    }

    [HttpGet("user/{id_readers}/history")]
    public async Task<IActionResult> GetRentalHistoryByUser(int id_readers)
    {
        return await _rentalService.GetRentalHistoryByUser(id_readers);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRentals()
    {
        return await _rentalService.GetCurrentRentals();
    }

    [HttpGet("book/{id_book}/history")]
    public async Task<IActionResult> GetRentalHistoryByBook(int id_book)
    {
        return await _rentalService.GetRentalHistoryByBook(id_book);
    }
}