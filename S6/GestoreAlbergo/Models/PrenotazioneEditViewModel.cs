using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestoreAlbergo.Models
{
    public class PrenotazioneEditViewModel
    {
        public Prenotazione Prenotazione { get; set; }
        public SelectList Clienti { get; set; }
        public SelectList Camere { get; set; }
    }
}
