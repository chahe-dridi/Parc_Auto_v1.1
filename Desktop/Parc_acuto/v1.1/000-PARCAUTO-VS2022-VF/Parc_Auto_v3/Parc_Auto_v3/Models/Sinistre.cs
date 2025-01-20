using System;
using System.ComponentModel.DataAnnotations;

namespace Parc_Auto_v3.Models
{
    public class Sinistre
    {
        public int Id { get; set; }

        [Display(Name = "Date de dommage")]
        [DataType(DataType.Date)]
        public DateTime DateDommage { get; set; }  

        [Display(Name = "Observation")]
        public string Observation { get; set; }  

        [Display(Name = "Description")]
        public string Description { get; set; }  

        [Display(Name = "Date de circulation")]
        [DataType(DataType.Date)]
        public DateTime Date_circulation { get; set; }  

        [Display(Name = "État de voiture")]
        public string EtatDeVoiture { get; set; }

        [Display(Name = "Prix fixe")]
        public long PrixFixe { get; set; }

        public int VoitureId { get; set; }
        public Voiture Voiture { get; set; }
    }
}
