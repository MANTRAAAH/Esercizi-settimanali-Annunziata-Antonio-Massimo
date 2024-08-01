namespace PizzeriaS7.Models
{
    public class UpdateIngredientiRequest
    {
        public int ProdottoId { get; set; } 
        public int IngredienteId { get; set; } 
        public bool Aggiunto { get; set; }  
        public int Quantity { get; set; } 
    }
}
