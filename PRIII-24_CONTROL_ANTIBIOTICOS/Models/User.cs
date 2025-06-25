using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUser { get; set; }

    public int? IdSpecialty { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
    public int? IdMedicalCouncil { get; set; }
    public string Role { get; set; } = null!;
    public bool? IsActive { get; set; }

    public virtual ICollection<AdministrationRequest> AdministrationRequests { get; set; } = new List<AdministrationRequest>();

    public virtual MedicalCouncil? IdMedicalCouncilNavigation { get; set; }

    public virtual Person? IdPersonNavigation { get; set; }

    public virtual MedicalSpecialty? IdSpecialtyNavigation { get; set; }
}
