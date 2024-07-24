namespace GestoreAlbergo.Models
{
    public class ServizioAggiuntivo
    {
        public int ID { get; set; }
        public int PrenotazioneID { get; set; }
        public DateTime Data { get; set; }
        public int Quantita { get; set; }
        public decimal Prezzo { get; set; }
        public int ListaServizioID { get; set; }
        public string Descrizione { get; set; }
        public string PrezzoFormatted { get; set; }
        public string TotaleFormatted { get; set; }
    }

}
