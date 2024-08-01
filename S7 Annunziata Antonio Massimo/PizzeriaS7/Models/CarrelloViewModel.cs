namespace PizzeriaS7.Models
{
public class CarrelloViewModel
{
    public Prodotto Prodotto { get; set; }
    public int Quantity { get; set; }
    public decimal PrezzoTotale { get; set; }
    public List<int> IngredientiAggiunti { get; set; } = new List<int>();
    public List<int> IngredientiBase { get; set; } = new List<int>();
    }
    

}
