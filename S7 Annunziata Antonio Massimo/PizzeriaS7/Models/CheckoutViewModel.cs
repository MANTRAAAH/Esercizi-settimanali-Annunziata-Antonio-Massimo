namespace PizzeriaS7.Models
{
    public class CheckoutViewModel
    {
        public Ordine Ordine { get; set; }
        public List<(Prodotto prodotto, int quantity)> CarrelloItems { get; set; }
    }
}
