using System.ComponentModel.DataAnnotations;

namespace Parc_Auto_v3.Models
{
    public class Modele
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public int MarqueId { get; set; }
        public Marque Marque { get; set; }

        public ICollection<Voiture> Voitures { get; set; }
    }
}
