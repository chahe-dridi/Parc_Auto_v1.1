using System;
using System.ComponentModel.DataAnnotations;

namespace Parc_Auto_v3.Models
{
    public class Vignette
    {
        public int Id { get; set; }

        [Display(Name = "Date d'échéance")]
        [DataType(DataType.Date)]
        public DateTime DateEchance { get; set; }

        [Display(Name = "Date valide")]
        [DataType(DataType.Date)]
        public DateTime DateValide { get; set; } 

        

        [Display(Name = "Prix unitaire")]
        public decimal PrixUnitaire { get; set; }

        public int VoitureId { get; set; }
        public Voiture Voiture { get; set; }
    }
 
}
