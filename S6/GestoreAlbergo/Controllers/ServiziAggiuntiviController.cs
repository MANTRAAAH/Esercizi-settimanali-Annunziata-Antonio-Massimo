using GestoreAlbergo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using GestoreAlbergo.Models;

namespace GestoreAlbergo.Controllers
{
    public class ServiziAggiuntiviController : Controller
    {
        private readonly DatabaseHelper _databaseHelper;

        public ServiziAggiuntiviController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        // Metodo per mostrare il form per aggiungere un servizio aggiuntivo
        [HttpGet]
        public IActionResult AggiungiServizioAggiuntivo(int prenotazioneId)
        {
            var model = new ServizioAggiuntivo { PrenotazioneID = prenotazioneId };
            return View(model);
        }

        // Metodo per aggiungere un servizio aggiuntivo
        [HttpPost]
        public IActionResult AggiungiServizioAggiuntivo(ServizioAggiuntivo servizio)
        {
            using (var connection = _databaseHelper.GetConnection())
            {
                var command = new SqlCommand(
                    "INSERT INTO ServiziAggiuntivi (PrenotazioneID, Data, Quantita, Prezzo, Descrizione) VALUES (@PrenotazioneID, @Data, @Quantita, @Prezzo, @Descrizione)",
                    connection);

                command.Parameters.AddWithValue("@PrenotazioneID", servizio.PrenotazioneID);
                command.Parameters.AddWithValue("@Data", servizio.Data);
                command.Parameters.AddWithValue("@Quantita", servizio.Quantita);
                command.Parameters.AddWithValue("@Prezzo", servizio.Prezzo);
                command.Parameters.AddWithValue("@Descrizione", servizio.Descrizione);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("DettagliPrenotazione", "Prenotazioni", new { id = servizio.PrenotazioneID });
        }
    }

}
