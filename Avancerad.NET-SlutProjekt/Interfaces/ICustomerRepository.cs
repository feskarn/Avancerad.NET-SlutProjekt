using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.DTO.Responses;
using Avancerad.NET_SlutProjekt.DTO;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDto>> GetAllCustomers();
        Task<IEnumerable<CustomerDto>> GetCustomers(string name, string email, string sortField, bool ascending);
        Task<CustomerDto> GetCustomerById(int customerId);
        Task<CreateCustomerResponse> AddCustomer(CreateCustomer customer);
        Task UpdateCustomer(CustomerDto updatedCustomer);

        Task DeleteCustomer(int customerId);
        Task<double> GetTotalBookingHoursForWeek(int customerId, int year, int weekNumber);
    }
}