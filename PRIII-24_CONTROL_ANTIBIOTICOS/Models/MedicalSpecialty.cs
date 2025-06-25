using System;
using System.Collections.Generic;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class MedicalSpecialty
{
    public int IdSpecialty { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
