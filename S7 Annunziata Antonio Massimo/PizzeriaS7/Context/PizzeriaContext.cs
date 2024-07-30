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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione dei tipi di colonna per le proprietà decimal
            modelBuilder.Entity<Prodotto>(entity =>
            {
                entity.Property(e => e.Prezzo).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Ordine>()
                            .HasMany(o => o.DettagliOrdine)
                            .WithOne(d => d.Ordine)
                            .HasForeignKey(d => d.OrdineId);

            modelBuilder.Entity<DettaglioOrdine>()
                .HasOne(d => d.Prodotto)
                .WithMany()
                .HasForeignKey(d => d.ProdottoId);
        }
    }
}
