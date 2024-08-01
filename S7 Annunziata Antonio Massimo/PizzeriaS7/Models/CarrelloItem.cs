using PizzeriaS7.Models;

public class CarrelloItem
{
    public int ProdottoId { get; set; }
    public int Quantity { get; set; }
    public List<int> IngredientiBaseIds { get; set; } = new List<int>();
    public List<int> IngredientiAggiuntiIds { get; set; } = new List<int>();

    public decimal CalcolaPrezzoTotale(decimal prezzoBase)
    {
        decimal prezzoIngredientiExtra = IngredientiAggiuntiIds.Count * 1.50m;
        return (prezzoBase + prezzoIngredientiExtra) * Quantity;
    }
}