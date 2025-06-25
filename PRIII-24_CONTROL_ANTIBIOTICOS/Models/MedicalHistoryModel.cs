namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models
{
    public class MedicalHistoryModel
    {
        public int IdMedicalHistory { get; set; }

        public decimal Weight { get; set; }

        public decimal Height { get; set; }

        public decimal Bmi { get; set; }

        public string Bmiclassification { get; set; } = null!;

        public int IdPatient { get; set; }

        public int IdDiagnosis { get; set; }

        public string ArchivoBase64 { get; set; }
        public string NombreArchivo { get; set; }
        public List<MedicacionCEA> Medicaciones { get; set; }
        public List<short> Allergies { get; set; }
        public List<int> Laboratories { get; set; }
        //actualice aqui
        public string Justificative { get; set; } = null!;
    }
    public class MedicacionCEA
    {
        public int idMedication { get; set; }
        public string Medicamento { get; set; }
        public decimal Dosis { get; set; }
        public string Frecuency { get; set;}
        public string Administration { get; set; }
    }
   
}
