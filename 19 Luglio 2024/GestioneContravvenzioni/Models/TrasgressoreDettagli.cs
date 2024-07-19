namespace GestioneContravvenzioni.Models
{
    public class TrasgressoreDettagli
    {
        public int Idanagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Città { get; set; }
        public string CAP { get; set; }
        public string Cod_Fisc { get; set; }
        public int TotaleVerbali { get; set; }
    }

}
