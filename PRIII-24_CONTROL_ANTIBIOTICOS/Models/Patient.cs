using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class Patient
{
    public int IdPatient { get; set; }

    [Display(Name = "Código de paciente")]
    public string PatientCode { get; set; } = null!;

    public virtual Person IdPersonNavigation { get; set; } = null!;

    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
}
