using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class MedicalCouncil
{
    public int IdMedicalCouncil { get; set; }

    public string? Name { get; set; }

    public int? IdUser { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
