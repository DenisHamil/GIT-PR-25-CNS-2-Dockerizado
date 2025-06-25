using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class Medication
{
    public int IdMedication { get; set; }

    public string Name { get; set; } = null!;

    public int? IdCategory { get; set; }

    public string Presentation { get; set; } = null!;

    public string Administration { get; set; } = null!;

    public virtual MedicationCategory? IdCategoryNavigation { get; set; }
}
