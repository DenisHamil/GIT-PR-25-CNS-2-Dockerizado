using System.ComponentModel.DataAnnotations;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models
{
    public class TreatmentObservation
    {
        public int IdTO { get; set; }

        [Required(ErrorMessage = "La observación es obligatoria.")]
        [StringLength(500, ErrorMessage = "La observación no puede superar los 500 caracteres.")]
        public string Observation { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Status { get; set; }
        public DateTime RegisterDate { get; set; }

        // Propiedad de navegación a MedicalHistory
        public virtual MedicalHistory? MedicalHistoryNavigation { get; set; }
    }
    
}
