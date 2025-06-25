using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class Diagnosis
{
    public int IdDiagnosis { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
}
