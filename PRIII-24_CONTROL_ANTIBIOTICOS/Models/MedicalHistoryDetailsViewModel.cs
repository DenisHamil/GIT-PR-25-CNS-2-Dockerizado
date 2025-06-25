namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models
{
    public class MedicalHistoryDetailsViewModel
    {
        public int IdMedicalHistory { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public decimal Bmi { get; set; }
        public string Bmiclassification { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Justificative { get; set; }
        public PatientViewModel Patient { get; set; }
        public List<AllergyViewModel> Allergies { get; set; }
        public string Diagnosis { get; set; }
        public List<MedicacionCEA> Medicaciones { get; set; }
        public int IdVote { get; set; }
        public List<VoteSumaryModel> Votes { get; set; }
        public List<RequestMeditacionCEA> RequestMedications { get; set; }

    }
    public class RequestMeditacionCEA
    {
        public decimal Dosage { get; set; }
        public string Frecuency { get; set; }
        public string Name { get; set; }
    }

    public class PatientViewModel
    {
        public int IdPatient { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Ci { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }

    public class AllergyViewModel
    {
        public short IdAllergy { get; set; }
        public string AllergyName { get; set; }
    }
    public class VoteSumaryModel
    {
        public string statusVote { get; set; }
        public decimal pointVote { get; set; }
        public string? Administration { get; set; }
        public string? Doses { get; set; }
        public string? Recommendation { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string? secondLastName { get; set; }
    }

}
