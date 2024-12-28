using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser_Website.Data;
using Hairdresser_Website.Models;

namespace Hairdresser_Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor, ApplicationDbContext DI ile enjekte edilir.
        public ApiController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Context null olmamalı
        }

        // GET: api/<CustomersApiController>
        [HttpGet("{id}")]
        public IActionResult Get()
        {
            return RedirectToAction();
        }

        // DELETE api/<CustomersApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // AppointmentId'ye göre siparişi bul
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
            {
                return NotFound(new { message = "Sipariş bulunamadı." });
            }

            // Siparişi sil
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sipariş başarıyla silindi." });
        }
    }
}
