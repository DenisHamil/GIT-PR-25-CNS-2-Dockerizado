using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class AdministrationRequest
{
    public int IdRequest { get; set; }

    public int? IdUser { get; set; }

    public string MedicalJustification { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public int IdMedicalHistory { get; set; }

    /// <summary>
    /// - Pendiente
    /// - Aprobado
    /// - Rechazado
    /// - Terminado
    /// </summary>
    public string Status { get; set; } = null!;

    public string? Response { get; set; }

    public DateTime? RegisterDate { get; set; }

    public List<RequestMedication>? RequestMedications { get; set; }

    // Propiedades de navegación
    public virtual MedicalHistory? IdMedicalHistoryNavigation { get; set; } = null!;
    public virtual User? IdUserNavigation { get; set; }
}
