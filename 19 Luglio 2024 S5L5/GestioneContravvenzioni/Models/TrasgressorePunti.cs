namespace GestioneContravvenzioni.Models
{
    public class TrasgressorePunti
    {
        public int Idanagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public int TotalePuntiDecurtati { get; set; }
    }

}
