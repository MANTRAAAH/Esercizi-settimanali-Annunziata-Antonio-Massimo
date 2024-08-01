namespace PizzeriaS7.Models
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string FotoUrl { get; set; }
        public decimal Prezzo { get; set; }
        public int TempoConsegna { get; set; }

        public ICollection<Ingrediente> Ingredienti { get; set; } = new List<Ingrediente>();

        // Proprietà non mappata al database, usata solo per la selezione degli ingredienti nel form
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public int[] IngredientiIds { get; set; }
    }
}
