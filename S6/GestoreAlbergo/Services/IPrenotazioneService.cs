using GestoreAlbergo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestoreAlbergo.Services
{
    public interface IPrenotazioneService
    {
        Task<IEnumerable<Prenotazione>> GetAllAsync();
        Task<Prenotazione> GetByIdAsync(int id);
        Task CreateAsync(Prenotazione prenotazione);
        Task UpdateAsync(Prenotazione prenotazione);
        Task DeleteAsync(int id);
        Task<int> GetNextProgressiveNumberAsync(int year);
        Task<IEnumerable<Prenotazione>> GetPrenotazioniByCodiceFiscaleAsync(string codiceFiscale);
        Task<IEnumerable<ServizioAggiuntivo>> GetServiziAggiuntiviByPrenotazioneIdAsync(int prenotazioneId);
    }
}
