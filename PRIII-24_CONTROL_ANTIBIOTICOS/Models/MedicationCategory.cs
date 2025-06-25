using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class MedicationCategory
{
    public int IdCategory { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Medication> Medications { get; set; } = new List<Medication>();
}
