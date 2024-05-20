using Avancerad.NET_SlutProjekt.DTO;

namespace Avancerad.NET_SlutProjekt.Interfaces
{
    public interface IChangeLogRepository
    {
        Task<IEnumerable<ChangeLogDto>> ChangeLogHistory();
    }
}

