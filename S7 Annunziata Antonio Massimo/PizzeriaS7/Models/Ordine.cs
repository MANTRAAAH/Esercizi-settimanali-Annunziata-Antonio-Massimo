namespace PizzeriaS7.Models
{
    public class Ordine
    {
        public int Id { get; set; }
        public string UtenteId { get; set; } // Usa string se IdentityUser.Id è stringa
        public DateTime DataOrdine { get; set; }
        public string IndirizzoSpedizione { get; set; }
        public string Note { get; set; }
        public bool Evaso { get; set; }
        public virtual ICollection<DettaglioOrdine> DettagliOrdine { get; set; }
    }
}
