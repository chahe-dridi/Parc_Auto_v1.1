using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parc_Auto_v3.Models
{
    public class Demandes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; }

        [Required]
        [StringLength(50)]  
        public string IdEmploye { get; set; }  

        [Required]
        [StringLength(100)]
        public string AffectationDepartement { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeVoiture { get; set; }

        [Required]
        [StringLength(255)]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Date Depart")]
        [DataType(DataType.Date)]
        public DateTime DateDepart { get; set; }

        [Required]
        [Display(Name = "Date Arrivée")]
        [DataType(DataType.Date)]
        public DateTime DateArrivee { get; set; }


        [Required]
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        [StringLength(255)]
        public string Accompagnateur { get; set; }


        [Required]
        [StringLength(255)]
        public string Mission { get; set; }

        public int? VoitureId { get; set; }

 
        [ForeignKey("VoitureId")]
        public virtual Voiture Voiture { get; set; }

     
        [StringLength(50)]
        public string? Etat { get; set; }  


        public int? Kilometrage { get; set; } // Nullable integer property


        public string UserId { get; set; }

    
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

    }



}
