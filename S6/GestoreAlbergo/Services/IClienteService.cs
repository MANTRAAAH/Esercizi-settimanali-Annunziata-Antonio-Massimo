using GestoreAlbergo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestoreAlbergo.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientiAsync();
        Task<Cliente> GetByIdAsync(int id);
        Task CreateAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(int id);
        Task<Cliente> GetByCodiceFiscaleAsync(string codiceFiscale);
        string CalcolaCodiceFiscale(Cliente cliente);

        // Add this method
        Task<IEnumerable<Cliente>> SearchClientiAsync(string query);
    }
}
