using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Infra.Interfaces
{
    public interface ILoteRepository
    {
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        Task<Lote> GetLoteByIdsAsync(int EventoId, int LoteId);
    }
}