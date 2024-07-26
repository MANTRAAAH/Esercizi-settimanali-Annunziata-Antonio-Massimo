namespace GestoreAlbergo.Models
{
    public class Prenotazione
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public string CodiceFiscale { get; set; }
        public Cliente? Cliente { get; set; }
        public int? CameraId { get; set; }
        public Camera? Camera { get; set; }
        public DateTime DataPrenotazione { get; set; }
        public int NumeroProgressivo { get; set; }
        public int Anno { get; set; }
        public DateTime PeriodoDal { get; set; }
        public DateTime PeriodoAl { get; set; }
        public decimal Caparra { get; set; }
        public decimal TariffaApplicata { get; set; }
        public string Dettagli { get; set; } 
        public ICollection<ServizioAggiuntivo>? ServiziAggiuntivi { get; set; }
    }
}
