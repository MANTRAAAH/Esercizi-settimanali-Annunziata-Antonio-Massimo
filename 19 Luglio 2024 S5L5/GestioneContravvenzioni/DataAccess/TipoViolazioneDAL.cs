using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GestioneContravvenzioni.Models;
using Microsoft.Extensions.Configuration;

public class TipoViolazioneDAL
{
    private readonly string _connectionString;

    public TipoViolazioneDAL(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<TipoViolazione>> GetTipiViolazioneAsync()
    {
        var tipiViolazione = new List<TipoViolazione>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SELECT * FROM TIPO_VIOLAZIONE", connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tipiViolazione.Add(new TipoViolazione
                        {
                            Idviolazione = reader.GetInt32(0),
                            Descrizione = reader.GetString(1),
                            Importo = reader.GetDecimal(2),
                            DecurtamentoPunti = reader.GetInt32(3),
                            RitiroPatente = reader.GetBoolean(4),
                            IsContestabile = reader.GetBoolean(5)
                        });
                    }
                }
            }
        }

        return tipiViolazione;
    }

    public async Task<TipoViolazione> GetTipoViolazioneByIdAsync(int id)
    {
        TipoViolazione tipoViolazione = null;

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SELECT * FROM TIPO_VIOLAZIONE WHERE Idviolazione = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        tipoViolazione = new TipoViolazione
                        {
                            Idviolazione = reader.GetInt32(0),
                            Descrizione = reader.GetString(1),
                            Importo = reader.GetDecimal(2),
                            DecurtamentoPunti = reader.GetInt32(3),
                            RitiroPatente = reader.GetBoolean(4),
                            IsContestabile = reader.GetBoolean(5)
                        };
                    }
                }
            }
        }

        return tipoViolazione;
    }


    public async Task CreateTipoViolazioneAsync(TipoViolazione tipoViolazione)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("INSERT INTO TIPO_VIOLAZIONE (Descrizione, Importo, DecurtamentoPunti, RitiroPatente, IsContestabile) VALUES (@Descrizione, @Importo, @DecurtamentoPunti, @RitiroPatente, @IsContestabile)", connection))
            {
                command.Parameters.AddWithValue("@Descrizione", tipoViolazione.Descrizione);
                command.Parameters.AddWithValue("@Importo", tipoViolazione.Importo);
                command.Parameters.AddWithValue("@DecurtamentoPunti", tipoViolazione.DecurtamentoPunti);
                command.Parameters.AddWithValue("@RitiroPatente", tipoViolazione.RitiroPatente);
                command.Parameters.AddWithValue("@IsContestabile", tipoViolazione.IsContestabile);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateTipoViolazioneAsync(TipoViolazione tipoViolazione)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("UPDATE TIPO_VIOLAZIONE SET Descrizione = @Descrizione, Importo = @Importo, DecurtamentoPunti = @DecurtamentoPunti, RitiroPatente = @RitiroPatente, IsContestabile = @IsContestabile WHERE Idviolazione = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", tipoViolazione.Idviolazione);
                command.Parameters.AddWithValue("@Descrizione", tipoViolazione.Descrizione);
                command.Parameters.AddWithValue("@Importo", tipoViolazione.Importo);
                command.Parameters.AddWithValue("@DecurtamentoPunti", tipoViolazione.DecurtamentoPunti);
                command.Parameters.AddWithValue("@RitiroPatente", tipoViolazione.RitiroPatente);
                command.Parameters.AddWithValue("@IsContestabile", tipoViolazione.IsContestabile);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteTipoViolazioneAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("DELETE FROM TIPO_VIOLAZIONE WHERE Idviolazione = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
