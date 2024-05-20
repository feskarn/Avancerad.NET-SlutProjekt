using AutoMapper;
using Avancerad.NET_SlutProjekt.Data;
using Avancerad.NET_SlutProjekt.DTO;
using Avancerad.NET_SlutProjekt.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Avancerad.NET_SlutProjekt.Repositories
{
    public class ChangeLogRepository : IChangeLogRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ChangeLogRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChangeLogDto>> ChangeLogHistory()
        {
            var changeLogs = await _context.ChangeLogs.ToListAsync();

            return _mapper.Map<IEnumerable<ChangeLogDto>>(changeLogs);
        }

    }
}
