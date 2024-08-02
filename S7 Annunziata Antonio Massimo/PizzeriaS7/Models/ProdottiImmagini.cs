namespace PizzeriaS7.Models
{
    public class ProdottiImmagini
    {
        public int Id { get; set; }
        public int ProdottoId { get; set; }
        public string ImmagineUrl { get; set; }

        public Prodotto Prodotto { get; set; }
    }

}
