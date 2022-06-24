using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Infra
{
    public interface IPalestranteRepository
    {

        Task<Palestrante[]> GetAllPalestranteByNameAsync(string name, bool includeEventos);
        Task<Palestrante[]> GetAllPalestranteAsync(string tema, bool includeEventos);
        Task<Palestrante> GetAllPalestranteByIdAsync(int PalestranteId, bool includeEventos);
    }
}