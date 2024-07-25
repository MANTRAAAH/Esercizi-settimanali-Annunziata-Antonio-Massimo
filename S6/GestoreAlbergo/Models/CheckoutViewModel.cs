namespace GestoreAlbergo.Models
{
    public class CheckoutViewModel
    {
        public Cliente Cliente { get; set; }
        public Prenotazione Prenotazione { get; set; }
        public List<ServizioAggiuntivo> ServiziAggiuntivi { get; set; }
        public decimal TotaleDaSaldare { get; set; }
        public string TotaleDaSaldareFormatted { get; set; }
    }
}
