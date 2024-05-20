using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.DTO.Responses;
using Avancerad.NET_SlutProjekt.DTO;
using Avancerad.NET_SlutProjekt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avancerad.NET_SlutProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyAppointmentRepository _appointmentRepository;

        public CompanyController(ICompanyRepository companyRepository, ICompanyAppointmentRepository appointmentRepository)
        {
            _companyRepository = companyRepository;
            _appointmentRepository = appointmentRepository;
        }


        [HttpGet("AllCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                return Ok(await _companyRepository.GetAllCompanies());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        [HttpPost("CreateCompany")]

        [ProducesResponseType(200, Type = typeof(CreateCompanyResponse))]
        public async Task<IActionResult> CreateCompany(CreateCompany request)
        {
            try
            {
                return Ok(await _companyRepository.AddCompany(request));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data to the database");

            }
        }

        [HttpPost("AddAppointment")]
        [Authorize(Policy = "CompanyPolicy")]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDto appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest("Invalid input to the appointment");
                }
                var addedAppointment = await _appointmentRepository.AddCompanyAppointment(appointment);
                return CreatedAtAction(nameof(AddAppointment), new { id = addedAppointment.AppointId }, addedAppointment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data to the database");
            }
        }
        [HttpPut("UpdateAppointment/{appointId:int}")]
        [Authorize(Policy = "CompanyPolicy")]
        public async Task<IActionResult> UpdateAppointment(int appointId, [FromBody] AppointmentDto appointment)
        {
            try
            {
                var updatedAppointment = await _appointmentRepository.UpdateCompanyAppointment(appointment);
                if (updatedAppointment == null)
                {
                    return NotFound("Appointment not found");
                }
                return Ok(updatedAppointment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error update data from the database");
            }
        }
        [HttpDelete("DeleteAppointment/{appointId:int}")]
        [Authorize(Policy = "CompanyPolicy")]
        public async Task<IActionResult> DeleteAppointment(int appointId)
        {
            try
            {
                var deletedAppointment = await _appointmentRepository.DeleteCompanyAppointment(appointId);
                if (deletedAppointment == null)
                {
                    return NotFound("Appointment to delete was not found");
                }
                return Ok(deletedAppointment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data from the database");
            }
        }
        [HttpGet("GetAppointmentsByDateRange")]
        [Authorize(Policy = "CompanyPolicy")]
        public async Task<IActionResult> GetAppointmentsByDateRange(DateTime startDate, DateTime endDate, int companyId)
        {
            try
            {
                var appointments = await _companyRepository.GetAppointmentsByDateRange(startDate, endDate, companyId);
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound("No appointments found for the given date range and company.");
                }
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database");
            }
        }
    }
}