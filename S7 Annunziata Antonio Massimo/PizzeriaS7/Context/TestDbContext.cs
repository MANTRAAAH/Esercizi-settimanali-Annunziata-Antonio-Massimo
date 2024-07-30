using PizzeriaS7.Context;
using PizzeriaS7.Models;
using Microsoft.EntityFrameworkCore;
public class TestDbContext
{
    private readonly PizzeriaContext _context;

    public TestDbContext(PizzeriaContext context)
    {
        _context = context;
    }

    public async Task InserisciProdotto()
    {
        var prodotto = new Prodotto
        {
            Nome = "Margherita",
            FotoUrl = "url_to_image",
            Prezzo = 7.50m,
            TempoConsegna = 30,
            Ingredienti = "Pomodoro, Mozzarella, Basilico"
        };

        _context.Prodotti.Add(prodotto);
        await _context.SaveChangesAsync();

        Console.WriteLine("Prodotto inserito con successo!");
    }

    public async Task InserisciOrdine()
    {
        var ordine = new Ordine
        {
            UtenteId = "1",
            DataOrdine = DateTime.Now,
            IndirizzoSpedizione = "Via Roma 123",
            Note = "Senza glutine",
            Evaso = false,
            DettagliOrdine = new List<DettaglioOrdine>
            {
                new DettaglioOrdine
                {
                    ProdottoId = 1, // ID di un prodotto esistente
                    Quantità = 2,
                    PrezzoTotale = 15.00m
                }
            }
        };

        _context.Ordini.Add(ordine);
        await _context.SaveChangesAsync();

        Console.WriteLine("Ordine inserito con successo!");
    }

    public async Task LeggiProdotti()
    {
        var prodotti = await _context.Prodotti.ToListAsync();

        foreach (var prodotto in prodotti)
        {
            Console.WriteLine($"Nome: {prodotto.Nome}, Prezzo: {prodotto.Prezzo}");
        }
    }
}
