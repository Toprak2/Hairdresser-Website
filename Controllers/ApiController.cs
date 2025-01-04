using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hairdresser_Website.Data;
using Hairdresser_Website.Models;
using System.Security.Claims;

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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string userId = User.FindFirstValue("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Kullanıcıya ait randevuları al
            var appointments = await _context.Appointments
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    a.AppointmentDate,
                    a.AppointmentId
                    
                })
                .ToListAsync();

            if (!appointments.Any())
            {
                return NotFound(new { message = "Randevu bulunamadı." });
            }

            // Randevuları dizi olarak döndür
            return Ok(appointments);
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
