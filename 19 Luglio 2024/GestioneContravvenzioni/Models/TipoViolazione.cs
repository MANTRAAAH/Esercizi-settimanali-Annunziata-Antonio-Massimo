namespace GestioneContravvenzioni.Models
{
    public class TipoViolazione
    {
        public int Idviolazione { get; set; }
        public string Descrizione { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
        public bool RitiroPatente { get; set; }
        public bool IsContestabile { get; set; }
    }

}
