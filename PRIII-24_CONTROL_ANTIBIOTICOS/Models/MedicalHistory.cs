using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class MedicalHistory
{
    public int IdMedicalHistory { get; set; }

    public decimal Weight { get; set; }

    public decimal Height { get; set; }

    public decimal Bmi { get; set; }

    public string Bmiclassification { get; set; } = null!;

    public int IdPatient { get; set; }

    public int IdDiagnosis { get; set; }

    public DateTime RegisterDate { get; set; }

    public byte[]? Archivepdf { get; set; }

    public string? Justificative { get; set; }

    public virtual ICollection<AdministrationRequest> AdministrationRequests { get; set; } = new List<AdministrationRequest>();

    public virtual Diagnosis? IdDiagnosisNavigation { get; set; } = null!;

    public virtual Patient? IdPatientNavigation { get; set; } = null!;

    // Propiedad de navegación para TreatmentObservation
    public virtual TreatmentObservation TreatmentObservation { get; set; }

    //actualice aqui
}
