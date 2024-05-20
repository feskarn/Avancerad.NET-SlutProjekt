using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.DTO.Responses;
using Avancerad.NET_SlutProjekt.DTO;
using Avancerad.NET_SlutProjekt.Interfaces;
using ClassLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Avancerad.NET_SlutProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAppointmentRepository _appointmentRepository;

        public CustomerController(ICustomerRepository customerRepository, ICustomerAppointmentRepository appointmentRepository)
        {
            _customerRepository = customerRepository;
            _appointmentRepository = appointmentRepository;
        }
        private IActionResult CheckAuthorization()
        {
            if (!User.IsInRole("Customer") && !User.IsInRole("Company"))
            {
                return Unauthorized(new { message = "User is not authorized to use this function" });
            }
            return null;
        }

        [HttpGet("AllCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                return Ok(await _customerRepository.GetAllCustomers());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database");
            }
        }
        [HttpGet("GetSortedCustomers")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> GetCustomers(string name, string email, string sortField, bool ascending = true)
        {
            var authorizationResult = CheckAuthorization();
            if (authorizationResult != null)
            {
                return authorizationResult;
            }
            try
            {
                var customers = await _customerRepository.GetCustomers(name, email, sortField, ascending);
                if (customers == null || customers.Count() == 0)
                {
                    return NotFound("No customers found for the given criteria.");
                }
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {ex.Message}");
            }
        }

        [HttpGet("CustomerById/ {customerId:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> GetSpecific(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerById(customerId);
                if (customer != null)
                {
                    return Ok(customer);
                }
                return NotFound($"Customer with ID {customerId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database");
            }
        }

        [HttpGet("{customerId}/bookings/{year}/{weekNumber}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> GetBookingHoursForWeek(int customerId, int year, int weekNumber)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerById(customerId);
                if (customer != null)
                {
                    var totalHours = await _customerRepository.GetTotalBookingHoursForWeek(customerId, year, weekNumber);
                    return Ok(new { CustomerId = customerId, Year = year, WeekNumber = weekNumber, TotalHours = totalHours });
                }
                return NotFound($"Customer with ID {customerId} not found.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database");
            }
        }

        [HttpPost("CreateCustomer")]

        [ProducesResponseType(200, Type = typeof(CreateCustomerResponse))]
        public async Task<IActionResult> CreateCustomer(CreateCustomer request)
        {
            try
            {
                return Ok(await _customerRepository.AddCustomer(request));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("UpdateCustomer/{customerId:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        public async Task<IActionResult> UpdateCustomer(int customerId, CustomerDto customerToUpdate)
        {
            if (customerId != customerToUpdate.CustomerId)
            {
                return BadRequest("Not a match with the ID and the customers ID");
            }
            try
            {
                await _customerRepository.UpdateCustomer(customerToUpdate);
                return Ok(customerToUpdate);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error trying to update data from the database");
            }
        }
        [HttpDelete("DeleteCustomer/{customerId:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {

            try
            {
                await _customerRepository.DeleteCustomer(customerId);
                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(500, "Error trying to delete customer from database");
            }
        }

        [HttpPost("AddAppointment")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> AddAppointment([FromBody] AppointmentDto appointment)
        {
            try
            {
                if (appointment == null)
                {
                    return BadRequest("Invalid input to the appointment");
                }
                var addedAppointment = await _appointmentRepository.AddCustomerAppointment(appointment);
                return CreatedAtAction(nameof(AddAppointment), new { id = addedAppointment.AppointId }, addedAppointment);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data to the database");
            }
        }
        [HttpPut("UpdateAppointment/{appointId:int}")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> UpdateAppointment(int appointId, [FromBody] AppointmentDto appointment)
        {
            try
            {
                var updatedAppointment = await _appointmentRepository.UpdateCustomerAppointment(appointment);
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
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> DeleteAppointment(int appointId)
        {
            try
            {
                var deletedAppointment = await _appointmentRepository.DeleteCustomerAppointment(appointId);
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
    }
}
