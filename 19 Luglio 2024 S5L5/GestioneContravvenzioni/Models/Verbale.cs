namespace GestioneContravvenzioni.Models
{
    public class Verbale
    {
        public int Idverbale { get; set; }
        public DateTime DataViolazione { get; set; }
        public string IndirizzoViolazione { get; set; }
        public string Nominativo_Agente { get; set; }
        public DateTime DataTrascrizioneVerbale { get; set; }
        public decimal Importo { get; set; }
        public int DecurtamentoPunti { get; set; }
        public int? Idanagrafica { get; set; }
        public int? Idviolazione { get; set; }
    }

}
