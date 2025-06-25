using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class Laboratory
{
    public int IdLaboratory { get; set; }

    public string TestName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public string Range { get; set; } = null!;
}
