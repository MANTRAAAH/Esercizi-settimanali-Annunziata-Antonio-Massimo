using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GestoreAlbergo.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public CheckoutController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet]
        public IActionResult GetCheckoutDetails(int prenotazioneId)
        {
            var prenotazione = new Prenotazione();
            var serviziAggiuntivi = new List<ServizioAggiuntivo>();
            decimal totaleServiziAggiuntivi = 0;

            using (var connection = _databaseHelper.GetConnection())
            {
                // Ottenere i dettagli della prenotazione
                var prenotazioneCommand = new SqlCommand(
                    "SELECT * FROM Prenotazioni WHERE ID = @ID",
                    connection);
                prenotazioneCommand.Parameters.AddWithValue("@ID", prenotazioneId);

                connection.Open();
                using (var reader = prenotazioneCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        prenotazione.Id = (int)reader["ID"];
                        prenotazione.Cliente.CodiceFiscale = (string)reader["CodiceFiscale"];
                        prenotazione.Camera.Numero = (int)reader["NumeroCamera"];
                        prenotazione.DataPrenotazione = (DateTime)reader["DataPrenotazione"];
                        prenotazione.NumeroProgressivo = (int)reader["NumeroProgressivo"];
                        prenotazione.Anno = (int)reader["Anno"];
                        prenotazione.PeriodoDal = (DateTime)reader["Dal"];
                        prenotazione.PeriodoAl = (DateTime)reader["Al"];
                        prenotazione.Caparra = (decimal)reader["Caparra"];
                        prenotazione.TariffaApplicata = (decimal)reader["Tariffa"];
                        prenotazione.Dettagli = reader["Dettagli"] as string;
                    }
                }

                // Ottenere i servizi aggiuntivi della prenotazione
                var serviziCommand = new SqlCommand(
                    "SELECT * FROM ServiziAggiuntivi WHERE PrenotazioneID = @PrenotazioneID",
                    connection);
                serviziCommand.Parameters.AddWithValue("@PrenotazioneID", prenotazioneId);

                using (var reader = serviziCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var servizio = new ServizioAggiuntivo
                        {
                            ID = (int)reader["ID"],
                            PrenotazioneID = (int)reader["PrenotazioneID"],
                            Data = (DateTime)reader["Data"],
                            Quantita = (int)reader["Quantita"],
                            Prezzo = (decimal)reader["Prezzo"],
                            Descrizione = (string)reader["Descrizione"]
                        };
                        serviziAggiuntivi.Add(servizio);
                        totaleServiziAggiuntivi += servizio.Prezzo * servizio.Quantita;
                    }
                }
            }

            // Calcolare l'importo totale da saldare
            var totale = prenotazione.TariffaApplicata - prenotazione.Caparra + totaleServiziAggiuntivi;

            // Restituire i dettagli del checkout
            var checkoutDetails = new
            {
                NumeroCamera = prenotazione.Camera.Numero,
                Periodo = new { Dal = prenotazione.PeriodoDal, Al = prenotazione.PeriodoAl },
                TariffaApplicata = prenotazione.TariffaApplicata,
                ServiziAggiuntivi = serviziAggiuntivi,
                TotaleDaSaldare = totale
            };

            return Json(checkoutDetails);
        }
    }

}
