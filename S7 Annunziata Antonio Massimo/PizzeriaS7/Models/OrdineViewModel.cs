﻿namespace PizzeriaS7.Models
{
    public class OrdineViewModel
    {
        public int OrdineId { get; set; }
        public DateTime DataOrdine { get; set; }
        public string NomeUtente { get; set; }
        public string IndirizzoSpedizione { get; set; }
        public string Note { get; set; }
        public bool Evaso { get; set; }
        public List<DettaglioOrdineViewModel> DettagliOrdine { get; set; }
        public decimal TotaleOrdine
        {
            get
            {
                // Calcola il totale sommando i prezzi dei dettagli dell'ordine
                return DettagliOrdine?.Sum(d => d.PrezzoTotale) ?? 0m;
            }
        }
    }

}
