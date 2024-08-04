﻿namespace PizzeriaS7.Models
{
    public class Ordine
    {
        public int Id { get; set; }
        public string UtenteId { get; set; }  
        public Utente Utente { get; set; }
        public DateTime DataOrdine { get; set; }
        public string IndirizzoSpedizione { get; set; }
        public string Note { get; set; }
        public bool Evaso { get; set; }
        public ICollection<DettaglioOrdine> DettagliOrdine { get; set; } = new List<DettaglioOrdine>();
    }
}
