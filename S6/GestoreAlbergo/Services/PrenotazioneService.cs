using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GestoreAlbergo.Models;

namespace GestoreAlbergo.Services
{
    public class PrenotazioneService : IPrenotazioneService
    {
        private readonly string _connectionString;
        private readonly IClienteService _clienteService;
        private readonly ICameraService _cameraService;

        public PrenotazioneService(string connectionString, IClienteService clienteService, ICameraService cameraService)
        {
            _connectionString = connectionString;
            _clienteService = clienteService;
            _cameraService = cameraService;
        }

        public async Task<IEnumerable<Prenotazione>> GetAllAsync()
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Prenotazioni", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var prenotazione = new Prenotazione
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                            NumeroCamera = reader.GetInt32(reader.GetOrdinal("NumeroCamera")),
                            DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                            PeriodoDal = reader.GetDateTime(reader.GetOrdinal("Dal")),
                            PeriodoAl = reader.GetDateTime(reader.GetOrdinal("Al")),
                            Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                            TariffaApplicata = reader.GetDecimal(reader.GetOrdinal("Tariffa")),
                            Dettagli = reader.IsDBNull(reader.GetOrdinal("Dettagli")) ? null : reader.GetString(reader.GetOrdinal("Dettagli")),
                            Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo"))
                        };

                        // Load related Cliente and Camera objects
                        prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
                        prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.NumeroCamera);

                        prenotazioni.Add(prenotazione);
                    }
                }
            }

            return prenotazioni;
        }

        public async Task<Prenotazione> GetByIdAsync(int id)
        {
            Prenotazione prenotazione = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        prenotazione = new Prenotazione
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                            NumeroCamera = reader.GetInt32(reader.GetOrdinal("NumeroCamera")),
                            DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                            PeriodoDal = reader.GetDateTime(reader.GetOrdinal("Dal")),
                            PeriodoAl = reader.GetDateTime(reader.GetOrdinal("Al")),
                            Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                            TariffaApplicata = reader.GetDecimal(reader.GetOrdinal("Tariffa")),
                            Dettagli = reader.IsDBNull(reader.GetOrdinal("Dettagli")) ? null : reader.GetString(reader.GetOrdinal("Dettagli")),
                            Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo"))
                        };

                        // Load related Cliente and Camera objects
                        prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
                        prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.NumeroCamera);
                    }
                }
            }

            return prenotazione;
        }

        public async Task CreateAsync(Prenotazione prenotazione)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    INSERT INTO Prenotazioni (CodiceFiscale, NumeroCamera, DataPrenotazione, NumeroProgressivo, Anno, Dal, Al, Caparra, Tariffa, Dettagli)
                    VALUES (@CodiceFiscale, @NumeroCamera, @DataPrenotazione, @NumeroProgressivo, @Anno, @Dal, @Al, @Caparra, @Tariffa, @Dettagli)", connection);

                command.Parameters.AddWithValue("@CodiceFiscale", prenotazione.CodiceFiscale);
                command.Parameters.AddWithValue("@NumeroCamera", prenotazione.NumeroCamera);
                command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                command.Parameters.AddWithValue("@Dal", prenotazione.PeriodoDal);
                command.Parameters.AddWithValue("@Al", prenotazione.PeriodoAl);
                command.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                command.Parameters.AddWithValue("@Tariffa", prenotazione.TariffaApplicata);
                command.Parameters.AddWithValue("@Dettagli", prenotazione.Dettagli);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Prenotazione prenotazione)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand(@"
                    UPDATE Prenotazioni
                    SET CodiceFiscale = @CodiceFiscale,
                        NumeroCamera = @NumeroCamera,
                        DataPrenotazione = @DataPrenotazione,
                        NumeroProgressivo = @NumeroProgressivo,
                        Anno = @Anno,
                        Dal = @Dal,
                        Al = @Al,
                        Caparra = @Caparra,
                        Tariffa = @Tariffa,
                        Dettagli = @Dettagli
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", prenotazione.Id);
                command.Parameters.AddWithValue("@CodiceFiscale", prenotazione.CodiceFiscale);
                command.Parameters.AddWithValue("@NumeroCamera", prenotazione.NumeroCamera);
                command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                command.Parameters.AddWithValue("@Dal", prenotazione.PeriodoDal);
                command.Parameters.AddWithValue("@Al", prenotazione.PeriodoAl);
                command.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                command.Parameters.AddWithValue("@Tariffa", prenotazione.TariffaApplicata);
                command.Parameters.AddWithValue("@Dettagli", prenotazione.Dettagli);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("DELETE FROM Prenotazioni WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> GetNextProgressiveNumberAsync(int anno)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = new SqlCommand("SELECT ISNULL(MAX(NumeroProgressivo), 0) + 1 FROM Prenotazioni WHERE Anno = @Anno", connection);
                command.Parameters.AddWithValue("@Anno", anno);

                return (int)await command.ExecuteScalarAsync();
            }
        }
    }
}
