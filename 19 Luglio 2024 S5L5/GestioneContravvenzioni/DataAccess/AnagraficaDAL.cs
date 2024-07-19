using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GestioneContravvenzioni.Models;
using Microsoft.Extensions.Configuration;

public class AnagraficaDAL
{
    private readonly string _connectionString;

    public AnagraficaDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<Anagrafica>> GetAnagraficheAsync()
    {
        var anagrafiche = new List<Anagrafica>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SELECT * FROM ANAGRAFICA", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        anagrafiche.Add(new Anagrafica
                        {
                            Idanagrafica = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Indirizzo = reader.GetString(3),
                            Città = reader.GetString(4),
                            CAP = reader.GetString(5),
                            Cod_Fisc = reader.GetString(6)
                        });
                    }
                }
            }
        }

        return anagrafiche;
    }

    public async Task CreateAnagraficaAsync(Anagrafica anagrafica)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Città, CAP, Cod_Fisc) VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @Cod_Fisc)", connection))
            {
                command.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                command.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                command.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                command.Parameters.AddWithValue("@Città", anagrafica.Città);
                command.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                command.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<Anagrafica> GetAnagraficaByIdAsync(int id)
    {
        Anagrafica anagrafica = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SELECT * FROM ANAGRAFICA WHERE Idanagrafica = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        anagrafica = new Anagrafica
                        {
                            Idanagrafica = reader.GetInt32(0),
                            Cognome = reader.GetString(1),
                            Nome = reader.GetString(2),
                            Indirizzo = reader.GetString(3),
                            Città = reader.GetString(4),
                            CAP = reader.GetString(5),
                            Cod_Fisc = reader.GetString(6)
                        };
                    }
                }
            }
        }

        return anagrafica;
    }

    public async Task UpdateAnagraficaAsync(Anagrafica anagrafica)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UPDATE ANAGRAFICA SET Cognome = @Cognome, Nome = @Nome, Indirizzo = @Indirizzo, Città = @Città, CAP = @CAP, Cod_Fisc = @Cod_Fisc WHERE Idanagrafica = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", anagrafica.Idanagrafica);
                command.Parameters.AddWithValue("@Cognome", anagrafica.Cognome);
                command.Parameters.AddWithValue("@Nome", anagrafica.Nome);
                command.Parameters.AddWithValue("@Indirizzo", anagrafica.Indirizzo);
                command.Parameters.AddWithValue("@Città", anagrafica.Città);
                command.Parameters.AddWithValue("@CAP", anagrafica.CAP);
                command.Parameters.AddWithValue("@Cod_Fisc", anagrafica.Cod_Fisc);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

}
