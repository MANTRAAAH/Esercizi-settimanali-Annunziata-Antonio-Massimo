namespace GestioneContravvenzioni.Models
{
    public class Anagrafica
    {
        public int Idanagrafica { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Indirizzo { get; set; }
        public string Città { get; set; }
        public string CAP { get; set; }
        public string Cod_Fisc { get; set; }
        public string NomeCompleto
        {
            get
            {
                return $"{Nome} {Cognome}";
            }
        }
    }


}
