using System;
using System.Globalization;

namespace Esercizio_S1_Back_End
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Inserisci i dati del contribuente:");

            // Richiesta e lettura dei dati base del contribuente
            Console.Write("Nome: ");
            string nome = Console.ReadLine() ?? string.Empty;

            Console.Write("Cognome: ");
            string cognome = Console.ReadLine() ?? string.Empty;

            // Richiesta e validazione della data di nascita
            DateTime dataDiNascita = default;
            bool dataValida = false;
            while (!dataValida)
            {
                Console.Write("Data di Nascita (dd/MM/yyyy): ");
                string inputData = Console.ReadLine() ?? string.Empty;
                try
                {
                    dataDiNascita = DateTime.ParseExact(inputData, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dataValida = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Formato data non valido. Si prega di inserire la data nel formato dd/MM/yyyy.");
                }
            }

            // Richiesta e validazione del codice fiscale
            Console.Write("Codice Fiscale: ");
            string codiceFiscale = Console.ReadLine() ?? string.Empty;
            while (codiceFiscale.Length != 16)
            {
                Console.WriteLine("Il codice fiscale deve essere di 16 caratteri.");
                codiceFiscale = Console.ReadLine() ?? string.Empty;
            }

            // Creazione dell'oggetto contribuente
            Contribuente contribuente = new Contribuente(nome, cognome, dataDiNascita, codiceFiscale, string.Empty, 0);

            // Richiesta e validazione del sesso
            bool sessoValido = false;
            while (!sessoValido)
            {
                try
                {
                    Console.Write("Sesso (M/F): ");
                    string inputSesso = Console.ReadLine() ?? string.Empty;
                    contribuente.ImpostaSessoDaStringa(inputSesso);
                    sessoValido = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // Richiesta del comune di residenza
            Console.Write("Comune di Residenza: ");
            contribuente.comuneDiResidenza = Console.ReadLine() ?? string.Empty;

            // Richiesta e validazione del reddito annuale
            Console.Write("Reddito Annuale: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal redditoAnnuale))
            {
                Console.WriteLine("Input non valido per il reddito annuale. Impostato a 0 per default.");
                redditoAnnuale = 0;
            }
            contribuente.redditoAnnuale = redditoAnnuale;

            // Calcolo e stampa dell'imposta da versare
            decimal impostaDaVersare = contribuente.CalcolaImposta();
            Console.WriteLine("\n==================================================\n");
            Console.WriteLine("CALCOLO DELL'IMPOSTA DA VERSARE:\n");
            Console.WriteLine($"Contribuente: {contribuente.Nome} {contribuente.Cognome}, \n");
            Console.WriteLine($"nato il {dataDiNascita.ToString("dd/MM/yyyy")} ({contribuente.SessoContribuente}), \n");
            Console.WriteLine($"residente in {contribuente.comuneDiResidenza}, \n");
            Console.WriteLine($"codice fiscale: {contribuente.CodiceFiscale}\n");
            Console.WriteLine($"Reddito dichiarato: €{contribuente.redditoAnnuale.ToString("N2")}\n");
            Console.WriteLine($"IMPOSTA DA VERSARE: €{impostaDaVersare.ToString("N2")}");
        }
    }
}
