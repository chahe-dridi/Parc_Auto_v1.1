using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Parc_Auto_v3.Models
{
    public class Voiture
    {
        public int Id { get; set; }

        [Required]
        public string Matricule { get; set; }

        [Required]
        public string TypeVoiture { get; set; }

        public int MarqueId { get; set; }
        public Marque Marque { get; set; }

        public int ModeleId { get; set; }
        public Modele Modele { get; set; }

        [Required]
        public int Km { get; set; }

        [Required]
        public string Carburant { get; set; }


		[Required]
		[StringLength(17, MinimumLength = 17, ErrorMessage = "Numero Serie Carte Grise must be exactly 17 characters.")]
		public string NumeroSerieCarteGrise { get; set; }



		[Required]
        public string Disponibilite { get; set; }

        public ICollection<VisiteTechnique> VisiteTechniques { get; set; }
        public ICollection<Vidange> Vidanges { get; set; }
        public ICollection<Sinistre> Sinistres { get; set; }
        public ICollection<Assurance> Assurances { get; set; }
        public ICollection<Vignette> Vignettes { get; set; }

        public ICollection<Demandes> Demandes { get; set; }

    }
}
