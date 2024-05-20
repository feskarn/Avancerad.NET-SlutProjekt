using Avancerad.NET_SlutProjekt.DTO.Requests;
using Avancerad.NET_SlutProjekt.DTO.Responses;
using Avancerad.NET_SlutProjekt.DTO;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<AppointmentDto>> GetAppointmentsByDateRange(DateTime startDate, DateTime endDate, int companyId);
        Task<IEnumerable<CompanyDto>> GetAllCompanies();
        Task<CreateCompanyResponse> AddCompany(CreateCompany company);

    }
}