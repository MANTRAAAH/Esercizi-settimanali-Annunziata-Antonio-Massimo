using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using GestioneContravvenzioni.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;


public class ReportController : Controller
{
    private readonly string _connectionString;

    public ReportController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
   public IActionResult Index()
    {
        return View();
    }


    // GET: Report/TotaleVerbaliPerTrasgressore, restituisce il numero di verbali per ogni trasgressore
    public async Task<IActionResult> TotaleVerbaliPerTrasgressore()
    {
        var report = new List<TrasgressoreDettagli>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
                SELECT 
                    a.Idanagrafica,
                    a.Cognome,
                    a.Nome,
                    a.Indirizzo,
                    a.Città,
                    a.CAP,
                    a.Cod_Fisc,
                    COUNT(v.Idverbale) AS TotaleVerbali
                FROM 
                    ANAGRAFICA a
                LEFT JOIN 
                    VERBALE v ON a.Idanagrafica = v.Idanagrafica
                GROUP BY 
                    a.Idanagrafica,
                    a.Cognome,
                    a.Nome,
                    a.Indirizzo,
                    a.Città,
                    a.CAP,
                    a.Cod_Fisc
                ORDER BY 
                    a.Cognome, a.Nome;", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            report.Add(new TrasgressoreDettagli
                            {
                                Idanagrafica = reader.GetInt32(0),
                                Cognome = reader.GetString(1),
                                Nome = reader.GetString(2),
                                Indirizzo = reader.GetString(3),
                                Città = reader.GetString(4),
                                CAP = reader.GetString(5),
                                Cod_Fisc = reader.GetString(6),
                                TotaleVerbali = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }

        return View(report);
    }


    // GET: Report/TotalePuntiDecurtatiPerTrasgressore, restituisce il totale dei punti decurtati per ogni trasgressore
    public async Task<IActionResult> TotalePuntiDecurtatiPerTrasgressore()
    {
        var report = new List<TrasgressorePunti>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
            SELECT 
                a.Idanagrafica,
                ISNULL(a.Cognome, 'Unknown') AS Cognome,
                ISNULL(a.Nome, 'Unknown') AS Nome,
                ISNULL(SUM(v.DecurtamentoPunti), 0) AS TotalePuntiDecurtati
            FROM 
                ANAGRAFICA a
            LEFT JOIN 
                VERBALE v ON a.Idanagrafica = v.Idanagrafica
            GROUP BY 
                a.Idanagrafica, a.Cognome, a.Nome
            ORDER BY 
                a.Cognome, a.Nome;", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var idanagrafica = reader.GetInt32(0);
                            var cognome = reader.GetString(1);
                            var nome = reader.GetString(2);
                            var totalePuntiDecurtati = reader.GetInt32(3);

                            report.Add(new TrasgressorePunti
                            {
                                Idanagrafica = idanagrafica,
                                Cognome = cognome,
                                Nome = nome,
                                TotalePuntiDecurtati = totalePuntiDecurtati
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore Server: {ex.Message}");
        }

        return View(report);
    }


    // GET: Report/ViolazioniConPuntiSuperioriADieci, restituisce le violazioni con decurtamento punti superiore a 10
    public async Task<IActionResult> ViolazioniConPuntiSuperioriADieci()
    {
        var report = new List<ViolazioneDettagli>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
                SELECT 
                    v.Importo,
                    a.Cognome,
                    a.Nome,
                    v.DataViolazione,
                    v.DecurtamentoPunti
                FROM 
                    VERBALE v
                JOIN 
                    ANAGRAFICA a ON v.Idanagrafica = a.Idanagrafica
                WHERE 
                    v.DecurtamentoPunti > 10
                ORDER BY 
                    v.DataViolazione;", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            report.Add(new ViolazioneDettagli
                            {
                                Importo = reader.GetDecimal(0),
                                Cognome = reader.GetString(1),
                                Nome = reader.GetString(2),
                                DataViolazione = reader.GetDateTime(3),
                                DecurtamentoPunti = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Errore Server");
        }

        return View(report);
    }

    //GET: Report/ViolazioniConImportoSuperioreAQuattrocento, restituisce le violazioni con importo superiore a 400
    public async Task<IActionResult> ViolazioniConImportoSuperioreAQuattrocento()
    {
        var report = new List<ViolazioneDettagli>();

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(@"
                SELECT 
                    v.Importo,
                    a.Cognome,
                    a.Nome,
                    v.DataViolazione,
                    v.DecurtamentoPunti
                FROM 
                    VERBALE v
                JOIN 
                    ANAGRAFICA a ON v.Idanagrafica = a.Idanagrafica
                WHERE 
                    v.Importo > 400
                ORDER BY 
                    v.Importo DESC;", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            report.Add(new ViolazioneDettagli
                            {
                                Importo = reader.GetDecimal(0),
                                Cognome = reader.GetString(1),
                                Nome = reader.GetString(2),
                                DataViolazione = reader.GetDateTime(3),
                                DecurtamentoPunti = reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Errore Server");
        }

        return View(report);
    }

}
