using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzeriaS7.Models;

namespace PizzeriaS7.Context
{
    public class PizzeriaContext : IdentityDbContext<Utente>
    {
        public PizzeriaContext(DbContextOptions<PizzeriaContext> options) : base(options) { }

        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<DettaglioOrdine> DettagliOrdine { get; set; }
        public DbSet<Utente> Utenti { get; set; }
        public DbSet<ProdottoImmagine> ProdottiImmagini { get; set; }
        public DbSet<Ingrediente> Ingredienti { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione dei tipi di colonna per le proprietà decimal
            modelBuilder.Entity<Prodotto>(entity =>
            {
                entity.Property(e => e.Prezzo).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Ordine>()
                .HasOne(o => o.Utente) // Relazione molti-a-uno tra Ordine e Utente
                .WithMany() // Non c'è una collezione di Ordini in Utente
                .HasForeignKey(o => o.UtenteId); // Chiave esterna in Ordine che punta a Utente

            // Configurazione esistente tra Ordine e DettaglioOrdine
            modelBuilder.Entity<Ordine>()
                .HasMany(o => o.DettagliOrdine)
                .WithOne(d => d.Ordine)
                .HasForeignKey(d => d.OrdineId);

            // Configurazione esistente tra DettaglioOrdine e Prodotto
            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Prodotto)
                .WithMany()
                .HasForeignKey(d => d.ProdottoId);

            // Configurazione della relazione molti-a-molti tra Prodotti e Ingredienti
            modelBuilder.Entity<Prodotto>()
                .HasMany(p => p.Ingredienti)
                .WithMany(i => i.Prodotti)
                .UsingEntity<Dictionary<string, object>>(
                    "ProdottoIngredienti",
                    j => j
                        .HasOne<Ingrediente>()
                        .WithMany()
                        .HasForeignKey("IngredienteId")
                        .OnDelete(DeleteBehavior.Cascade), // Cancellazione a cascata per gli ingredienti
                    j => j
                        .HasOne<Prodotto>()
                        .WithMany()
                        .HasForeignKey("ProdottoId")
                        .OnDelete(DeleteBehavior.Cascade) // Cancellazione a cascata per i prodotti
                );


        }
    }
}
