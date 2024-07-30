using PizzeriaS7.Models;

public class DettaglioOrdine
{
    public int Id { get; set; }
    public int OrdineId { get; set; }
    public int ProdottoId { get; set; }
    public int Quantità { get; set; }
    public decimal PrezzoTotale { get; set; }

    // Proprietà di navigazione
    public Ordine Ordine { get; set; }
    public Prodotto Prodotto { get; set; }
}
