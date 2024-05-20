using AutoMapper;
using Avancerad.NET_SlutProjekt.Data;
using Avancerad.NET_SlutProjekt.DTO;
using Avancerad.NET_SlutProjekt.Interfaces;
using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Avancerad.NET_SlutProjekt.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                                                 .Include(a => a.Customer)
                                                 .Include(a => a.Company)
                                                 .ToListAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointments(DateTime? startDate, DateTime? endDate, int? companyId, string sortField, bool ascending)
        {

            var query = _context.Appointments
                                .Include(a => a.Customer)
                                .Include(a => a.Company)
                                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(a => a.StartingTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.StartingTime <= endDate.Value);
            }

            if (companyId.HasValue)
            {
                query = query.Where(a => a.Company.CompanyId == companyId.Value);
            }

            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(Appointment), "a");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda(property, parameter);

                var methodName = ascending ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { query.ElementType, property.Type }, query.Expression, lambda);
                query = query.Provider.CreateQuery<Appointment>(resultExpression);
            }

            var appointments = await query.ToListAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

    }
}