using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestioneContravvenzioni.Models
{
    public class VerbaleCreateViewModel
    {
        public VerbaleCreateViewModel()
        {
            Verbale = new Verbale();
            AnagraficaList = new List<SelectListItem>();
            TipoViolazioneList = new List<SelectListItem>();
        }

        public Verbale Verbale { get; set; }
        public IEnumerable<SelectListItem>? AnagraficaList { get; set; }
        public IEnumerable<SelectListItem>? TipoViolazioneList { get; set; }
    }
}
