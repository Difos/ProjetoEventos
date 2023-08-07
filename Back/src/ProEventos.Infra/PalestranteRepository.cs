using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Infra.Context;

namespace ProEventos.Infra
{
    public class PalestranteRepository : IPalestranteRepository
    {
        public readonly InfraContext _context;
        public PalestranteRepository(InfraContext context)
        {
            _context = context;
        }
         public async Task<Palestrante[]> GetAllPalestranteAsync(string tema, bool includeEventos = false)
        {
            try
            {
                IQueryable<Palestrante> query = _context.Palestrantes
                   .Include(p => p.RedesSociais);

                if (includeEventos)
                {
                    query
                        .Include(p => p.PalestrantesEventos)
                        .ThenInclude(e => e.Evento);
                }

                query = query.AsNoTracking().OrderBy(e => e.Id);
                return await query.ToArrayAsync();
            }
            catch (System.Exception)
            {

                return default;
            }
        }

        public async Task<Palestrante[]> GetAllPalestranteByNameAsync(string name, bool includeEventos)
        {
            try
            {
                IQueryable<Palestrante> query = _context.Palestrantes
                   .Include(p => p.RedesSociais);

                if (includeEventos)
                {
                    query
                        .Include(p => p.PalestrantesEventos)
                        .ThenInclude(p => p.Evento);
                }

                query = query.AsNoTracking().OrderBy(p => p.Id).Where(
                    p => p.User.PrimeiroNome.ToLower().Contains(name.ToLower()) && 
                         p.User.UltimoNome.ToLower().Contains(name.ToLower()));
                return await query.ToArrayAsync();
            }
            catch (System.Exception)
            {

                return default;
            }
        }

         public async Task<Palestrante> GetAllPalestranteByIdAsync(int PalestranteId, bool includeEventos)
        {
            try
            {
                IQueryable<Palestrante> query = _context.Palestrantes
                   .Include(p => p.RedesSociais);

                if (includeEventos)
                {
                    query
                        .Include(p => p.PalestrantesEventos)
                        .ThenInclude(p => p.Evento);
                }

                query = query.AsNoTracking().OrderBy(p => p.Id).Where(p => p.Id == PalestranteId);
                return await query.FirstOrDefaultAsync();
            }
            catch (System.Exception)
            {

                return default;
            }
        }
    }
}