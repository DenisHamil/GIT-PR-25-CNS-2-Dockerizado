using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRIII_24_CONTROL_ANTIBIOTICOS.Models;

public partial class Person
{
    public int IdPerson { get; set; }

    [Display(Name = "Nombre(s)")]
    [Required(ErrorMessage = "El campo Nombres es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Name { get; set; } = null!;

    [Display(Name = "Primer apellido")]
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Segundo apellido")]
    [StringLength(100, ErrorMessage = "El segundo apellido no puede exceder los 100 caracteres")]
    public string? SecondLastName { get; set; }

    [Display(Name = "C.I.")]
    [Required(ErrorMessage = "El campo CI es obligatorio")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El CI debe contener solo números")]
    public string Ci { get; set; } = null!;


    [Display(Name = "Fecha de nacimiento")]
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DateRange(1900,2024, ErrorMessage = "La fecha debe estar entre 1900 y la fecha actual.")]
    public DateOnly DateOfBirth { get; set; }

    [Display(Name = "Sexo")]
    [Required(ErrorMessage = "El campo Género es obligatorio")]
    public string Gender { get; set; } = null!;

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

public class DateRangeAttribute : ValidationAttribute
{
    private readonly DateOnly _minDate;
    private readonly DateOnly _maxDate;

    public DateRangeAttribute(int minYear, int maxYear)
    {
        _minDate = new DateOnly(minYear, 1, 1);
        _maxDate = DateOnly.FromDateTime(DateTime.Today);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // Si el valor es nulo, permitir que otras validaciones (como Required) manejen este caso
        if (value == null)
            return ValidationResult.Success;

        if (value is DateOnly dateValue)
        {
            if (dateValue < _minDate || dateValue > _maxDate)
            {
                return new ValidationResult($"Inserte una fecha válida, por favor");
            }
        }
        else
        {
            return new ValidationResult("El formato de la fecha no es válido.");
        }

        return ValidationResult.Success;
    }
}



