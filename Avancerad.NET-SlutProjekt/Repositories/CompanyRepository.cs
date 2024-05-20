using AutoMapper;
using Avancerad.NET_SlutProjekt.Data;
using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.DTO.Responses;
using Avancerad.NET_SlutProjekt.DTO;
using Avancerad.NET_SlutProjekt.Interfaces;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace Avancerad.NET_SlutProjekt.Repositories
{
    public class CompanyRepository : ICompanyRepository, ICompanyAppointmentRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public CompanyRepository(AppDbContext context, IUserRepository userRepository, IMapper mapper)
        {
            _context = context;
            _userRepository = userRepository;
            _mapper = mapper;

        }



        public async Task<CreateCompanyResponse> AddCompany(CreateCompany company)
        {
            var user = new User()
            {
                Username = company.CompanyEmail,
                PasswordHash = company.Password,
                Role = "Company",
                IsActive = true
            };

            var userId = await _userRepository.CreateUser(user);
            var companyToInsert = new Company()
            {
                CompanyEmail = company.CompanyEmail,
                CompanyName = company.CompanyName,


                UserId = userId
            };
            await _context.Companies.AddAsync(companyToInsert);
            await _context.SaveChangesAsync();

            var companyResponse = new CreateCompanyResponse()
            {
                Username = company.CompanyEmail,
                Password = company.Password
            };

            return companyResponse;
        }



        public async Task<IEnumerable<CompanyDto>> GetAllCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return _mapper.Map<IEnumerable<CompanyDto>>(companies);

        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDateRange(DateTime startDate, DateTime endDate, int companyId)
        {
            var appointments = await _context.Appointments
                            .Include(a => a.Customer)
                            .Include(a => a.Company)
                            .Where(a => a.StartingTime >= startDate &&
                             a.StartingTime <= endDate &&
                             a.Company.CompanyId == companyId)
                                .ToListAsync();
            if (appointments == null)
            {
                return null;
            }
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }




      



        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                                                .Include(a => a.Customer)
                                                .Include(a => a.Company)
                                                .ToListAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> AddCompanyAppointment(AppointmentDto appointmentDto)
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
            await LogCompanyChange(result.Entity.AppointId,
                            appointmentDto.CustomerId,
                            appointmentDto.CompanyId,
                            "Appointment Created");
            
            var appointmentDtoResult = _mapper.Map<AppointmentDto>(result.Entity);
            appointmentDtoResult.CustomerId = appointmentDto.CustomerId;
            appointmentDtoResult.CompanyId = appointmentDto.CompanyId;
            return appointmentDtoResult;
        }

        public async Task<AppointmentDto> UpdateCompanyAppointment(AppointmentDto appointmentDto)
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
            await LogCompanyChange(appointment.AppointId,
                           appointment.Company.CompanyId,
                           appointment.Customer.CustomerId,
                           "Appointment Updated");
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> DeleteCompanyAppointment(int id)
        {
            var appointment = await _context.Appointments
                                    .Include(a => a.Company)
                                    .Include(a => a.Customer)
                                    .FirstOrDefaultAsync(a => a.AppointId == id);
            if (appointment == null)
            {
                return null;
            }

            await LogCompanyChange(appointment.AppointId,
                            appointment.Company?.CompanyId ?? 0,
                            appointment.Customer?.CustomerId ?? 0,
                            "Appointment Deleted");
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return _mapper.Map<AppointmentDto>(appointment);
        }
        public async Task LogCompanyChange(int appointmentId, int customerId, int companyId, string action)
        {
            var changeLog = new ChangeLog
            {
                AppointId = appointmentId,
                CustomerId = customerId,
                CompanyId = companyId,
                ChangedAtDate = DateTime.Now,
                Details = $"{action.ToUpper()} - AppointmentId: {appointmentId}, Issued By CompanyId: {companyId}, For CustomerId: {customerId}, LogTime: {DateTime.Now}"
            };
            await _context.ChangeLogs.AddAsync(changeLog);
            await _context.SaveChangesAsync();
        }








    }
}