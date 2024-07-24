using GestoreAlbergo.Models;

namespace GestoreAlbergo.Services
{
    public interface IListaServiziAggiuntiviService
    {
        Task<IEnumerable<ListaServiziAggiuntivi>> GetAllAsync();
    }
}
