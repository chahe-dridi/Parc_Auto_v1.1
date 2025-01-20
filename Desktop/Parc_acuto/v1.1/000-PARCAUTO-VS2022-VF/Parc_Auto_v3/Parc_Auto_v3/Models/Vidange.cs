using System;
using System.ComponentModel.DataAnnotations;

namespace Parc_Auto_v3.Models
{
    public class Vidange
    {
        public int Id { get; set; }

        [Display(Name = "Date d'intervention")]
        [DataType(DataType.Date)]
        public DateOnly DateIntervention { get; set; } 

        public string TypeIntervention { get; set; }
        public string Fournisseur { get; set; }
        public string NFacture { get; set; }
        public int Kilometrage { get; set; }
        public string OperationDetails { get; set; }
        public decimal MontantHt { get; set; }

        public int VoitureId { get; set; }
        public Voiture Voiture { get; set; }
    }
}
