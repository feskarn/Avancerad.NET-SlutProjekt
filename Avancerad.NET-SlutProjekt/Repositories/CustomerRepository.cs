using AutoMapper;
using Avancerad.NET_SlutProjekt.Data;
using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.DTO.Responses;
using Avancerad.NET_SlutProjekt.DTO;
using Avancerad.NET_SlutProjekt.Interfaces;
using ClassLibrary;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Avancerad.NET_SlutProjekt.Repositories
{
    public class CustomerRepository : ICustomerRepository, ICustomerAppointmentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public CustomerRepository(AppDbContext context, IUserRepository userRepository, IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<CustomerDto>> GetCustomers(string name, string email, string sortField, bool ascending)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(c => c.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Customer), "c");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = ascending ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type }, query.Expression, lambda);
                query = query.Provider.CreateQuery<Customer>(resultExpression);
            }

            var customers = await query.ToListAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        
        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }
        
        public async Task<CustomerDto> GetCustomerById(int customerId)
        {
            var customer = await _context.Customers
        .Include(c => c.Appointments)
            .ThenInclude(a => a.Company)
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return null;
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }


        
        public async Task<double> GetTotalBookingHoursForWeek(int customerId, int year, int weekNumber)
        {
            var firstDayOfWeek = ISOWeek.ToDateTime(year, weekNumber, DayOfWeek.Monday);
            var lastDayOfWeek = firstDayOfWeek.AddDays(6);

            var bookings = await _context.Appointments
                .Where(a => a.Customer.CustomerId == customerId
                            && a.StartingTime >= firstDayOfWeek
                            && a.StartingTime <= lastDayOfWeek)
                .ToListAsync();

            var totalMinutes = bookings.Sum(a => a.AppointmentDurationMinutes);
            var totalHours = totalMinutes / 60.0;
            totalHours = Math.Round(totalHours, 2);
            return totalHours;
        }

        
        public async Task<CreateCustomerResponse> AddCustomer(CreateCustomer customer)
        {
            var user = new User()
            {
                Username = customer.Email,
                PasswordHash = customer.Password,
                Role = "Customer",
                IsActive = true
            };

            var userId = await _userRepository.CreateUser(user);
            var customerToInsert = new Customer()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Phone = customer.Phone,
                UserId = userId
            };
            await _context.Customers.AddAsync(customerToInsert);
            await _context.SaveChangesAsync();

            var customerResponse = new CreateCustomerResponse()
            {
                Username = customer.Email,
                Password = customer.Password
            };

            return customerResponse;
        }
       
        public async Task UpdateCustomer(CustomerDto updatedCustomer)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == updatedCustomer.CustomerId);
            if (customer != null)
            {
                _mapper.Map(updatedCustomer, customer);
                customer.UpdateDate = DateTime.UtcNow;  

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Customer to update was not found");
            }
        }
        
        public async Task DeleteCustomer(int customerId)
        {
            var customerToDelete = _context.Customers
                                    .Include(c => c.Appointments)
                                    .FirstOrDefault(c => c.CustomerId == customerId);

            if (customerToDelete == null)
            {
                throw new KeyNotFoundException("The customer to delete was not found");

            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Appointments.RemoveRange(customerToDelete.Appointments);
                    _context.Customers.Remove(customerToDelete);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync(); 
                }
                catch
                {
                    await transaction.RollbackAsync();  
                    throw; 
                }
            }
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                                                 .Include(a => a.Customer)
                                                 .Include(a => a.Company)
                                                 .ToListAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> AddCustomerAppointment(AppointmentDto appointmentDto)
        {
            var company = await _context.Companies.FindAsync(appointmentDto.CompanyId);
            var customer = await _context.Customers.FindAsync(appointmentDto.CustomerId);

            if (company == null)
            {
                return null;
            }
            if (customer == null)
            {
                return null;
            }
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            appointment.CreationDate = DateTime.UtcNow;  
            appointment.UpdateDate = DateTime.UtcNow;
            appointment.Company = company;
            appointment.Customer = customer;

            var result = await _context.Appointments.AddAsync(appointment);

            await _context.SaveChangesAsync();
            await LogCustomerChange(result.Entity.AppointId,
                            appointmentDto.CustomerId,
                            appointmentDto.CompanyId,
                            "Appointment Created");

            var appointmentDtoResult = _mapper.Map<AppointmentDto>(result.Entity);
            appointmentDtoResult.CustomerId = appointmentDto.CustomerId;
            appointmentDtoResult.CompanyId = appointmentDto.CompanyId;
            return appointmentDtoResult;
        }

        public async Task<AppointmentDto> UpdateCustomerAppointment(AppointmentDto appointmentDto)
        {
            var appointment = await _context.Appointments
                                     .Include(a => a.Company)
                                     .Include(a => a.Customer)
                                     .FirstOrDefaultAsync(a => a.AppointId == appointmentDto.AppointId);
            if (appointment == null)
            {
                return null;
            }

            _mapper.Map(appointmentDto, appointment);
            appointment.UpdateDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await LogCustomerChange(appointment.AppointId,
                           appointment.Customer.CustomerId,
                           appointment.Company.CompanyId,
                           "Appointment Updated");
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> DeleteCustomerAppointment(int id)
        {
            var appointment = await _context.Appointments
                                     .Include(a => a.Company)
                                     .Include(a => a.Customer)
                                     .FirstOrDefaultAsync(a => a.AppointId == id);
            if (appointment == null)
            {
                return null;
            }

            await LogCustomerChange(appointment.AppointId,
                            appointment.Customer.CustomerId,
                            appointment.Company.CompanyId,
                            "Appointment Deleted");
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }


        public async Task LogCustomerChange(int appointmentId, int customerId, int companyId, string action)
        {


            var changeLog = new ChangeLog
            {
                AppointId = appointmentId,
                CustomerId = customerId,
                CompanyId = companyId,
                ChangedAtDate = DateTime.Now,
                Details = $"{action.ToUpper()} - AppointmentId: {appointmentId}, Issued By CustomerId: {customerId}, For CompanyId: {companyId}, LogTime: {DateTime.Now}"
            };
            await _context.ChangeLogs.AddAsync(changeLog);
            await _context.SaveChangesAsync();
        }
    }
}
