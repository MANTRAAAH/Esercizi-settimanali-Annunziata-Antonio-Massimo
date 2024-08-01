namespace PizzeriaS7.Models
{
    public class ProdottoIngrediente
    {
        public int ProdottoId { get; set; }
        public Prodotto Prodotto { get; set; }

        public int IngredienteId { get; set; }
        public Ingrediente Ingrediente { get; set; }

        public decimal CostoSupplementare { get; set; } = 1.50m; 
    }
}
