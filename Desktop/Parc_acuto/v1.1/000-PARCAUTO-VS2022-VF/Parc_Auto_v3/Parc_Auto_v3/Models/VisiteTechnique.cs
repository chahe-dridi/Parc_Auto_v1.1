using System.ComponentModel.DataAnnotations;

namespace Parc_Auto_v3.Models
{
    public class VisiteTechnique
    {
        public int Id { get; set; }


       
        [Display(Name = "Date d'échéance")]
        [DataType(DataType.Date)]
        public DateTime DateEchance { get; set; }



        [Display(Name = "Date Valide")]
        [DataType(DataType.Date)]
        public DateTime DateValide { get; set; }

      
        public decimal PrixUnitaire { get; set; }

        public int VoitureId { get; set; }
        public Voiture Voiture { get; set; }
    }
}
