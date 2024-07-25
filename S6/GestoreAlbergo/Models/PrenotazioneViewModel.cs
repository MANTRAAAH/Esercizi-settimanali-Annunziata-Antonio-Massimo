using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GestoreAlbergo.Models
{
    public class PrenotazioneViewModel
    {
        public PrenotazioneViewModel()
        {
            Prenotazione = new Prenotazione();
            Clienti = new List<SelectListItem>();
            Camere = new List<SelectListItem>();
        }

        public Prenotazione Prenotazione { get; set; }
        public IEnumerable<SelectListItem>? Clienti { get; set; }
        public IEnumerable<SelectListItem>? Camere { get; set; }

        // Aggiungi la proprietà CameraId
        public int CameraId { get; set; }
    }
}
