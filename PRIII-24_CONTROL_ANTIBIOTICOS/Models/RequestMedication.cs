namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models
{
    public class RequestMedication
    {
        public int IdMedication { get; set; }

        public int IdRequest { get; set; }

        public decimal Dosage { get; set; }

        public string Frecuency { get; set; } = null!;

        public string Administration { get; set; }

        public virtual Medication? IdMedicationNavigation { get; set; } = null!;
    }



}
