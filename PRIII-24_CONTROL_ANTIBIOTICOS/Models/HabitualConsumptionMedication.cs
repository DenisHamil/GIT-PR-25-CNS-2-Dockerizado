using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class HabitualConsumptionMedication
{
    public int IdMedication { get; set; }

    public int IdMedicalHistory { get; set; }

    public decimal Dosage { get; set; }

    public string Frecuency { get; set; } = null!;

    public string Administration { get; set; }

    public virtual MedicalHistory IdMedicalHistoryNavigation { get; set; } = null!;

    public virtual Medication IdMedicationNavigation { get; set; } = null!;
}
