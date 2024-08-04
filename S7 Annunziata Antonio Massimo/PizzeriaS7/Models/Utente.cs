using Microsoft.AspNetCore.Identity;

namespace PizzeriaS7.Models
{
    public class Utente : IdentityUser
    {
        public string Nome { get; set; }
        public string Ruolo { get; set; } = "User";
    }
}
