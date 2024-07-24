using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestoreAlbergo.Models
{
    public class AggiungiServizioViewModel
    {
        public int PrenotazioneID { get; set; }
        public List<SelectListItem> ListaServizi { get; set; }
        public int ListaServizioID { get; set; }
        public DateTime Data { get; set; }
        public int Quantita { get; set; }
    }
}
