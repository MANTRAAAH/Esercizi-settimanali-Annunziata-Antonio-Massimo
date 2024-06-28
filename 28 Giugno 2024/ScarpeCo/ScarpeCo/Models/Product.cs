using Microsoft.AspNetCore.Http; // Namespace necessario per utilizzare IFormFile, utilizzato per il caricamento delle immagini, che non sono riuscito ad implementare
using System;
using System.Collections.Generic;

namespace ScarpeCo.Models
{
    public class Product
    {
        public int Id { get; set; } // ID univoco del prodotto
        public string Name { get; set; } // Nome del prodotto
        public decimal Price { get; set; } // Prezzo del prodotto
        public string Description { get; set; } // Descrizione del prodotto
    }

    public static class ProductRepository
    {
        public static List<Product> Products { get; } = new List<Product>
        {
            // Lista statica di esempi di prodotti
            new Product
            {
                Id = 1,
                Name = "Scarpa A",
                Price = 100.00m,
                Description = "Descrizione dettagliata della Scarpa A"
            },
            new Product
            {
                Id = 2,
                Name = "Scarpa B",
                Price = 150.00m,
                Description = "Descrizione dettagliata della Scarpa B"
            }
        };
    }
}
