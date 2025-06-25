using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class MedicalHistoryLaboratory
{
    public int IdMedicalHistory { get; set; }

    public int IdLaboratory { get; set; }

    //public decimal Result { get; set; }

    //public virtual Laboratory IdLaboratoryNavigation { get; set; } = null!;

    //public virtual MedicalHistory IdMedicalHistoryNavigation { get; set; } = null!;
}
