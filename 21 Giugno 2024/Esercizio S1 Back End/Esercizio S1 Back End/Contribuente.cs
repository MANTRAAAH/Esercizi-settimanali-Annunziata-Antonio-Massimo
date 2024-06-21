using System;

namespace Esercizio_S1_Back_End
{
    internal class Contribuente
    {
        // Proprietà pubbliche per i dati del contribuente
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataDiNascita { get; set; }
        public string CodiceFiscale { get; set; }
        // Enumerazione per gestire il sesso del contribuente
        public enum Sesso { Maschio, Femmina }
        public Sesso SessoContribuente { get; private set; }
        public string comuneDiResidenza { get; set; }
        public decimal redditoAnnuale { get; set; }

        // Costruttore per inizializzare un nuovo oggetto Contribuente con i dati forniti
        public Contribuente(string nome, string cognome, DateTime dataDiNascita, string codiceFiscale, string comuneDiResidenza, decimal redditoAnnuale)
        {
            Nome = nome;
            Cognome = cognome;
            DataDiNascita = dataDiNascita;
            CodiceFiscale = codiceFiscale;
            this.comuneDiResidenza = comuneDiResidenza;
            this.redditoAnnuale = redditoAnnuale;
        }

        // Metodo per impostare il sesso del contribuente a partire da una stringa (M o F)
        public void ImpostaSessoDaStringa(string inputSesso)
        {
            if (inputSesso.ToUpper() == "M")
            {
                SessoContribuente = Sesso.Maschio;
            }
            else if (inputSesso.ToUpper() == "F")
            {
                SessoContribuente = Sesso.Femmina;
            }
            else
            {
                throw new ArgumentException("Input non valido. Si prega di inserire 'M' per Maschio o 'F' per Femmina.");
            }
        }

        // Metodo per calcolare l'imposta basata sul reddito annuale del contribuente
        public decimal CalcolaImposta()
        {
            // Scaglione fino a 15.000 euro
            if (redditoAnnuale <= 15000)
            {
                return redditoAnnuale * 0.23M; // Aliquota del 23%
            }
            // Scaglione da 15.001 a 28.000 euro
            else if (redditoAnnuale <= 28000)
            {
                return 3450 + ((redditoAnnuale - 15000) * 0.27M); // 3.450 euro + 27% sulla parte eccedente i 15.000 euro
            }
            // Scaglione da 28.001 a 55.000 euro
            else if (redditoAnnuale <= 55000)
            {
                return 6960 + ((redditoAnnuale - 28000) * 0.38M); // 6.960 euro + 38% sulla parte eccedente i 28.000 euro
            }
            // Scaglione da 55.001 a 75.000 euro
            else if (redditoAnnuale <= 75000)
            {
                return 17220 + ((redditoAnnuale - 55000) * 0.41M); // 17.220 euro + 41% sulla parte eccedente i 55.000 euro
            }
            // Scaglione oltre i 75.000 euro
            else
            {
                return 25420 + ((redditoAnnuale - 75000) * 0.43M); // 25.420 euro + 43% sulla parte eccedente i 75.000 euro
            }
        }
    }
}
