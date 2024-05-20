using Avancerad.NET_SlutProjekt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avancerad.NET_SlutProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        [HttpGet("AllAppointments")]
        [Authorize(Policy = "CustomerOrCompanyPolicy")]
        public async Task<IActionResult> GetAppointments()
        {
            try
            {
                return Ok(await _appointmentRepository.GetAllAppointments());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetSortedAppointments")]
        [Authorize(Policy = "CustomerOrCompanyPolicy")]
        public async Task<IActionResult> GetAppointments(DateTime? startDate, DateTime? endDate, int? companyId, string sortField, bool ascending = true)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointments(startDate, endDate, companyId, sortField, ascending);
                if (appointments == null || appointments.Count() == 0)
                {
                    return NotFound("No appointments found for the given criteria.");
                }
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }
    }
}

