namespace PizzeriaS7.Models
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string FotoUrl { get; set; }
        public decimal Prezzo { get; set; }
        public int TempoConsegna { get; set; }
        public string Ingredienti { get; set; }
    }
}
