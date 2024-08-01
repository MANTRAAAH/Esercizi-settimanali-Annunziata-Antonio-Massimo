using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaS7.Migrations
{
    /// <inheritdoc />
    public partial class PopulateIngredienti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Creazione della tabella Ingredienti
            migrationBuilder.CreateTable(
                name: "Ingredienti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredienti", x => x.Id);
                });

            // Inserimento degli ingredienti
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Mozzarella')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Pomodoro')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Basilico')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Funghi')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Prosciutto')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Salame')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Carciofi')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Olive')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Tonno')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Cipolle')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Gorgonzola')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Peperoni')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Salsiccia')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Melanzane')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Zucchine')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Rucola')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Parmigiano')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Speck')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Acciughe')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Capperi')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Patate')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Peperoncino')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Aglio')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Rosmarino')");
            migrationBuilder.Sql("INSERT INTO Ingredienti (Nome) VALUES ('Pancetta')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Cancellazione dei dati
            migrationBuilder.Sql("DELETE FROM Ingredienti WHERE Nome IN ('Mozzarella', 'Pomodoro', 'Basilico', 'Funghi', 'Prosciutto', 'Salame', 'Carciofi', 'Olive', 'Tonno', 'Cipolle', 'Gorgonzola', 'Peperoni', 'Salsiccia', 'Melanzane', 'Zucchine', 'Rucola', 'Parmigiano', 'Speck', 'Acciughe', 'Capperi', 'Patate', 'Peperoncino', 'Aglio', 'Rosmarino', 'Pancetta')");

            // Eliminazione della tabella Ingredienti
            migrationBuilder.DropTable(
                name: "Ingredienti");
        }
    }
}
