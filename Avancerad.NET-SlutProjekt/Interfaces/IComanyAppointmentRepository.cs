using Avancerad.NET_SlutProjekt.DTO;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface ICompanyAppointmentRepository
    {
        Task<AppointmentDto> AddCompanyAppointment(AppointmentDto entity);
        Task<AppointmentDto> UpdateCompanyAppointment(AppointmentDto entity);
        Task<AppointmentDto> DeleteCompanyAppointment(int id);
        Task LogCompanyChange(int appointmentId, int customerId, int companyId, string action);
    }
}
