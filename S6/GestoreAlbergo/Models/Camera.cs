namespace GestoreAlbergo.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Camera
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Numero Camera")]
        public int Numero { get; set; }

        [Required]
        [Display(Name = "Descrizione")]
        public string Descrizione { get; set; }

        [Required]
        [Display(Name = "Tipologia")]
        public string Tipologia { get; set; } // "Singola" o "Doppia"
    }

}
