using GestoreAlbergo.Models;
using System.Data.SqlClient;

namespace GestoreAlbergo.Services
{
    public class ListaServiziAggiuntiviService : IListaServiziAggiuntiviService
    {
        private readonly string _connectionString;
        private readonly ILogger<ListaServiziAggiuntiviService> _logger;

        public ListaServiziAggiuntiviService(string connectionString, ILogger<ListaServiziAggiuntiviService> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<IEnumerable<ListaServiziAggiuntivi>> GetAllAsync()
        {
            var servizi = new List<ListaServiziAggiuntivi>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM ListaServiziAggiuntivi", connection);
                connection.Open();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        servizi.Add(new ListaServiziAggiuntivi
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            NomeServizio = reader.GetString(reader.GetOrdinal("NomeServizio")),
                            Descrizione = reader.IsDBNull(reader.GetOrdinal("Descrizione")) ? null : reader.GetString(reader.GetOrdinal("Descrizione")),
                            Prezzo = reader.GetDecimal(reader.GetOrdinal("Prezzo"))
                        });
                    }
                }
            }

            return servizi;
        }
    }
}
