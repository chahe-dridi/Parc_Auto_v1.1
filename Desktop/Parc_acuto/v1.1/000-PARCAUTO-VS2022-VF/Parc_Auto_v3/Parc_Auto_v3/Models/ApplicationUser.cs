using Microsoft.AspNetCore.Identity;

namespace Parc_Auto_v3.Models
{
    public class ApplicationUser :IdentityUser
    {
        public virtual ICollection<Demandes> Demandes { get; set; }
    }
}
