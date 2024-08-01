namespace PizzeriaS7.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Prodotto> Prodotti { get; set; } = new List<Prodotto>();
    }


}
