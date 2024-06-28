using System;
using System.Collections.Generic;
using System.Linq;
using ScarpeCo.Models;

namespace ScarpeCo.Services
{
    public class ProductService
    {
        // Metodo per ottenere tutti i prodotti dal repository
        public List<Product> GetAllProducts()
        {
            return ProductRepository.Products; // Restituisce l'elenco completo dei prodotti dal repository
        }

        // Metodo per ottenere un singolo prodotto dal repository tramite ID
        public Product GetProductById(int id)
        {
            return ProductRepository.Products.FirstOrDefault(p => p.Id == id); // Cerca e restituisce il prodotto con l'ID specificato
        }

        // Metodo per aggiungere un nuovo prodotto al repository
        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product)); // Eccezione se il prodotto è nullo
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Il nome del prodotto è obbligatorio", nameof(product.Name));
                // Eccezione se il nome del prodotto è vuoto o contiene solo spazi
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Il prezzo del prodotto deve essere maggiore di zero", nameof(product.Price));
                // Eccezione se il prezzo del prodotto è non positivo
            }

            // Genera un ID univoco per il nuovo prodotto
            product.Id = GenerateUniqueId();

            // Aggiunge il prodotto al repository
            ProductRepository.Products.Add(product);
        }

        // Metodo privato per generare un ID univoco per il nuovo prodotto
        private int GenerateUniqueId()
        {
            // Se non ci sono prodotti nel repository, l'ID del nuovo prodotto sarà 1; altrimenti, sarà l'ID massimo + 1
            return !ProductRepository.Products.Any() ? 1 : ProductRepository.Products.Max(p => p.Id) + 1;
        }
    }
}
