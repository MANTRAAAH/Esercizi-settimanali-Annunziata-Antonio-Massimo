using GestoreAlbergo.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GestoreAlbergo.Services
{
    public class ClienteService : IClienteService
    {
        private readonly DatabaseHelper _databaseHelper;

        public ClienteService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientiAsync()
        {
            var clienti = new List<Cliente>();

            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand("SELECT * FROM Clienti", connection);
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clienti.Add(new Cliente
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                            Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Citta = reader.GetString(reader.GetOrdinal("Citta")),
                            Provincia = reader.GetString(reader.GetOrdinal("Provincia")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"))
                        });
                    }
                }
            }

            return clienti;
        }
        public async Task<IEnumerable<Cliente>> SearchClientiAsync(string query)
        {
            var clienti = new List<Cliente>();

            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand("SELECT * FROM Clienti WHERE Nome LIKE @Query OR Cognome LIKE @Query", connection);
                command.Parameters.AddWithValue("@Query", "%" + query + "%");
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clienti.Add(new Cliente
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                            Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Citta = reader.GetString(reader.GetOrdinal("Citta")),
                            Provincia = reader.GetString(reader.GetOrdinal("Provincia")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"))
                        });
                    }
                }
            }

            return clienti;
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            Cliente cliente = null;
            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand("SELECT * FROM Clienti WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        cliente = new Cliente
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                            Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Citta = reader.GetString(reader.GetOrdinal("Citta")),
                            Provincia = reader.GetString(reader.GetOrdinal("Provincia")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"))
                        };
                    }
                }
            }
            return cliente;
        }


        public async Task CreateAsync(Cliente cliente)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand(
                    "INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Citta, Provincia, Email, Telefono, Cellulare) VALUES (@CodiceFiscale, @Cognome, @Nome, @Citta, @Provincia, @Email, @Telefono, @Cellulare)",
                    connection);

                command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@Citta", cliente.Citta);
                command.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand(
                    "UPDATE Clienti SET CodiceFiscale = @CodiceFiscale, Cognome = @Cognome, Nome = @Nome, Citta = @Citta, Provincia = @Provincia, Email = @Email, Telefono = @Telefono, Cellulare = @Cellulare WHERE Id = @Id",
                    connection);

                command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@Citta", cliente.Citta);
                command.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);
                command.Parameters.AddWithValue("@Id", cliente.Id);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand("DELETE FROM Clienti WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<Cliente> GetByCodiceFiscaleAsync(string codiceFiscale)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Clienti WHERE CodiceFiscale = @CodiceFiscale", connection);
                command.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Cliente
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                            Citta = reader.GetString(reader.GetOrdinal("Citta")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Cellulare = reader.GetString(reader.GetOrdinal("Cellulare")),
                            Provincia = reader.GetString(reader.GetOrdinal("Provincia"))
                        };
                    }
                }
            }

            return null; // Return null if no client is found
        }


        public string CalcolaCodiceFiscale(Cliente cliente)
        {
            // Logica per calcolare il codice fiscale in base ai dati del cliente
            // Nota: Questo è solo un esempio e non rappresenta il calcolo reale del codice fiscale italiano

            return $"{cliente.Cognome.Substring(0, 3).ToUpper()}{cliente.Nome.Substring(0, 3).ToUpper()}1234X";
        }
    }
}
