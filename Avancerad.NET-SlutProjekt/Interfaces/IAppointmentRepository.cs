using Avancerad.NET_SlutProjekt.DTO;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<AppointmentDto>> GetAllAppointments();
        Task<IEnumerable<AppointmentDto>> GetAppointments(DateTime? startDate, DateTime? endDate, int? companyId, string sortField, bool ascending);
    }
}
