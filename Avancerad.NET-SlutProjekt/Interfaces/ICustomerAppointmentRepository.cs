using Avancerad.NET_SlutProjekt.DTO;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface ICustomerAppointmentRepository
    {
        Task<AppointmentDto> AddCustomerAppointment(AppointmentDto entity);
        Task<AppointmentDto> UpdateCustomerAppointment(AppointmentDto entity);
        Task<AppointmentDto> DeleteCustomerAppointment(int id);
        Task LogCustomerChange(int appointmentId, int customerId, int companyId, string action);
    }
}
