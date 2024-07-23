namespace GestoreAlbergo.Models
{
    public class CheckoutDetailsViewModel
    {
        public int NumeroCamera { get; set; }
        public DateTime PeriodoDal { get; set; }
        public DateTime PeriodoAl { get; set; }
        public decimal TariffaApplicata { get; set; }
        public List<ServizioAggiuntivo> ServiziAggiuntivi { get; set; }
        public decimal TotaleDaSaldare { get; set; }
    }

}
