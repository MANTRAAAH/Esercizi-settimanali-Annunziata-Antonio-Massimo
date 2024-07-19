using GestioneContravvenzioni.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class VerbaleDAL
{
    private readonly string _connectionString;

    public VerbaleDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<Verbale>> GetVerbaliAsync()
    {
        var verbali = new List<Verbale>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SELECT * FROM VERBALE", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        verbali.Add(new Verbale
                        {
                            Idverbale = reader.GetInt32(reader.GetOrdinal("Idverbale")),
                            DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                            IndirizzoViolazione = reader.GetString(reader.GetOrdinal("IndirizzoViolazione")),
                            Nominativo_Agente = reader.GetString(reader.GetOrdinal("Nominativo_Agente")),
                            DataTrascrizioneVerbale = reader.GetDateTime(reader.GetOrdinal("DataTrascrizioneVerbale")),
                            Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                            DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                            Idanagrafica = reader.GetInt32(reader.GetOrdinal("Idanagrafica")),
                            Idviolazione = reader.GetInt32(reader.GetOrdinal("Idviolazione"))
                        });
                    }
                }
            }
        }

        return verbali;
    }



    public async Task<Verbale> GetVerbaleByIdAsync(int id)
    {
        Verbale verbale = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("SELECT * FROM VERBALE WHERE Idverbale = @Idverbale", connection))
            {
                command.Parameters.AddWithValue("@Idverbale", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        verbale = new Verbale
                        {
                            Idverbale = reader.GetInt32(reader.GetOrdinal("Idverbale")),
                            DataViolazione = reader.GetDateTime(reader.GetOrdinal("DataViolazione")),
                            IndirizzoViolazione = reader.GetString(reader.GetOrdinal("IndirizzoViolazione")),
                            Nominativo_Agente = reader.GetString(reader.GetOrdinal("Nominativo_Agente")),
                            DataTrascrizioneVerbale = reader.GetDateTime(reader.GetOrdinal("DataTrascrizioneVerbale")),
                            Importo = reader.GetDecimal(reader.GetOrdinal("Importo")),
                            DecurtamentoPunti = reader.GetInt32(reader.GetOrdinal("DecurtamentoPunti")),
                            Idanagrafica = reader.IsDBNull(reader.GetOrdinal("Idanagrafica")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Idanagrafica")),
                            Idviolazione = reader.IsDBNull(reader.GetOrdinal("Idviolazione")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Idviolazione")),
                        };
                    }
                }
            }
        }

        return verbale;
    }


    public async Task CreateVerbaleAsync(Verbale verbale)
    {
        var query = "INSERT INTO Verbale (Idanagrafica, Idviolazione, DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti) VALUES (@Idanagrafica, @Idviolazione, @DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti)";

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Idanagrafica", verbale.Idanagrafica);
                command.Parameters.AddWithValue("@Idviolazione", verbale.Idviolazione);
                command.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                command.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                command.Parameters.AddWithValue("@Nominativo_Agente", verbale.Nominativo_Agente);
                command.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                command.Parameters.AddWithValue("@Importo", verbale.Importo);
                command.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
