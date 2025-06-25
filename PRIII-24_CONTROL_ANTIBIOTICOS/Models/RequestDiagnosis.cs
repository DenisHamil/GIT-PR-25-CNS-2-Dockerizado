using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class RequestDiagnosis
{
    public int RequestDiagnosisId { get; set; }

    public int? RequestId { get; set; }

    public int? DiagnosisId { get; set; }
}
