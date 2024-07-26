using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GestoreAlbergo.Models;
using Microsoft.Extensions.Logging;

namespace GestoreAlbergo.Services
{
    public class PrenotazioneService : IPrenotazioneService
    {
        private readonly string _connectionString;
        private readonly IClienteService _clienteService;
        private readonly ICameraService _cameraService;
        private readonly ILogger<PrenotazioneService> _logger;

        public PrenotazioneService(string connectionString, IClienteService clienteService, ICameraService cameraService, ILogger<PrenotazioneService> logger)
        {
            _connectionString = connectionString;
            _clienteService = clienteService;
            _cameraService = cameraService;
            _logger = logger;
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
                            CameraId = reader.IsDBNull(reader.GetOrdinal("CameraId")) ? null : reader.GetInt32(reader.GetOrdinal("CameraId")),
                            DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                            PeriodoDal = reader.GetDateTime(reader.GetOrdinal("Dal")),
                            PeriodoAl = reader.GetDateTime(reader.GetOrdinal("Al")),
                            Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                            TariffaApplicata = reader.GetDecimal(reader.GetOrdinal("Tariffa")),
                            Dettagli = reader.IsDBNull(reader.GetOrdinal("Dettagli")) ? null : reader.GetString(reader.GetOrdinal("Dettagli")),
                            Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo"))
                        };

                        prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
                        if (prenotazione.CameraId.HasValue)
                        {
                            prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.CameraId.Value);
                        }

                        prenotazioni.Add(prenotazione);
                    }
                }
            }

            return prenotazioni;
        }
        public async Task<IEnumerable<Prenotazione>> GetPrenotazioniByClienteIdAsync(int clienteId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE ClienteId = @ClienteId", connection);
                command.Parameters.AddWithValue("@ClienteId", clienteId);
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var prenotazioni = new List<Prenotazione>();
                    while (await reader.ReadAsync())
                    {
                        prenotazioni.Add(new Prenotazione
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        });
                    }
                    return prenotazioni;
                }
            }
        }


        public async Task<Prenotazione?> GetByIdAsync(int id)
        {
            Prenotazione? prenotazione = null;

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
                            CameraId = reader.IsDBNull(reader.GetOrdinal("CameraId")) ? null : reader.GetInt32(reader.GetOrdinal("CameraId")),
                            DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                            PeriodoDal = reader.GetDateTime(reader.GetOrdinal("Dal")),
                            PeriodoAl = reader.GetDateTime(reader.GetOrdinal("Al")),
                            Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                            TariffaApplicata = reader.GetDecimal(reader.GetOrdinal("Tariffa")),
                            Dettagli = reader.IsDBNull(reader.GetOrdinal("Dettagli")) ? null : reader.GetString(reader.GetOrdinal("Dettagli")),
                            Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo")),
                        };

                        prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
                        if (prenotazione.CameraId.HasValue)
                        {
                            prenotazione.Camera = await _cameraService.GetCameraByNumeroAsync(prenotazione.CameraId.Value);
                        }
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
            INSERT INTO Prenotazioni (CodiceFiscale, DataPrenotazione, NumeroProgressivo, Anno, Dal, Al, Caparra, Tariffa, Dettagli, CameraId)
            VALUES (@CodiceFiscale, @DataPrenotazione, @NumeroProgressivo, @Anno, @Dal, @Al, @Caparra, @Tariffa, @Dettagli, @CameraId)", connection);

                command.Parameters.AddWithValue("@CodiceFiscale", prenotazione.CodiceFiscale);
                command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                command.Parameters.AddWithValue("@Dal", prenotazione.PeriodoDal);
                command.Parameters.AddWithValue("@Al", prenotazione.PeriodoAl);
                command.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                command.Parameters.AddWithValue("@Tariffa", prenotazione.TariffaApplicata);
                command.Parameters.AddWithValue("@Dettagli", prenotazione.Dettagli ?? string.Empty);
                command.Parameters.AddWithValue("@CameraId", prenotazione.CameraId.HasValue ? prenotazione.CameraId.Value : DBNull.Value);

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
                command.Parameters.AddWithValue("@CameraId", prenotazione.CameraId);
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

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        var deleteServiziCommand = new SqlCommand("DELETE FROM ServiziAggiuntivi WHERE PrenotazioneID = @PrenotazioneID", connection, transaction);
                        deleteServiziCommand.Parameters.AddWithValue("@PrenotazioneID", id);
                        await deleteServiziCommand.ExecuteNonQueryAsync();


                        var deletePrenotazioneCommand = new SqlCommand("DELETE FROM Prenotazioni WHERE Id = @Id", connection, transaction);
                        deletePrenotazioneCommand.Parameters.AddWithValue("@Id", id);
                        await deletePrenotazioneCommand.ExecuteNonQueryAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task<IEnumerable<Prenotazione>> GetPrenotazioniByCodiceFiscaleAsync(string codiceFiscale)
        {
            _logger.LogInformation("Getting prenotazioni for CodiceFiscale: {CodiceFiscale}", codiceFiscale);
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE CodiceFiscale = @CodiceFiscale", connection);
                    command.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            _logger.LogInformation("Prenotazione found with Id: {Id}", reader.GetInt32(reader.GetOrdinal("Id")));
                            var prenotazione = new Prenotazione
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
                                CameraId = reader.IsDBNull(reader.GetOrdinal("CameraId")) ? null : reader.GetInt32(reader.GetOrdinal("CameraId")),
                                DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                                PeriodoDal = reader.GetDateTime(reader.GetOrdinal("Dal")),
                                PeriodoAl = reader.GetDateTime(reader.GetOrdinal("Al")),
                                Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                                TariffaApplicata = reader.GetDecimal(reader.GetOrdinal("Tariffa")),
                                Dettagli = reader.IsDBNull(reader.GetOrdinal("Dettagli")) ? null : reader.GetString(reader.GetOrdinal("Dettagli")),
                                Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                                NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo"))
                            };


                            prenotazione.Cliente = await _clienteService.GetByCodiceFiscaleAsync(prenotazione.CodiceFiscale);
                            if (prenotazione.CameraId.HasValue)
                            {
                                prenotazione.Camera = await _cameraService.GetCameraByIdAsync(prenotazione.CameraId.Value);
                            }

                            prenotazioni.Add(prenotazione);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while retrieving prenotazioni for CodiceFiscale: {CodiceFiscale}", codiceFiscale);
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }

            if (prenotazioni.Count == 0)
            {
                _logger.LogWarning("No prenotazioni found for CodiceFiscale: {CodiceFiscale}", codiceFiscale);
            }

            return prenotazioni;
        }

        public async Task<IEnumerable<ServizioAggiuntivo>> GetServiziAggiuntiviByPrenotazioneIdAsync(int prenotazioneId)
        {
            _logger.LogInformation("Getting servizi aggiuntivi for PrenotazioneID: {PrenotazioneID}", prenotazioneId);
            var serviziAggiuntivi = new List<ServizioAggiuntivo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand(
                    "SELECT * FROM ServiziAggiuntivi WHERE PrenotazioneID = @PrenotazioneID",
                    connection);
                command.Parameters.AddWithValue("@PrenotazioneID", prenotazioneId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var servizio = new ServizioAggiuntivo
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            PrenotazioneID = reader.GetInt32(reader.GetOrdinal("PrenotazioneID")),
                            Data = reader.GetDateTime(reader.GetOrdinal("Data")),
                            Quantita = reader.GetInt32(reader.GetOrdinal("Quantita")),
                            ListaServizioID = reader.GetInt32(reader.GetOrdinal("ListaServizioID"))
                        };

                        serviziAggiuntivi.Add(servizio);
                    }
                }


                foreach (var servizio in serviziAggiuntivi)
                {
                    var listaServizioCommand = new SqlCommand(
                        "SELECT * FROM ListaServiziAggiuntivi WHERE ID = @ID",
                        connection);
                    listaServizioCommand.Parameters.AddWithValue("@ID", servizio.ListaServizioID);

                    using (var listaServizioReader = await listaServizioCommand.ExecuteReaderAsync())
                    {
                        if (await listaServizioReader.ReadAsync())
                        {
                            servizio.Descrizione = (string)listaServizioReader["Descrizione"];
                            servizio.Prezzo = (decimal)listaServizioReader["Prezzo"];
                        }
                    }
                }
            }

            return serviziAggiuntivi;
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
        public async Task<Cliente> GetClienteByPrenotazioneIdAsync(int prenotazioneId)
        {
            Cliente cliente = null;
            var query = "SELECT c.* FROM Clienti c JOIN Prenotazioni p ON c.CodiceFiscale = p.CodiceFiscale WHERE p.Id = @PrenotazioneId";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PrenotazioneId", prenotazioneId);
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
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
            }

            return cliente;
        }

    }
}
