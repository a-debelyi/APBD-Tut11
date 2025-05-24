using System.ComponentModel.DataAnnotations;

namespace APBD_11.DTOs;

public class PatientRequestDto
{
    [Required] public int IdPatient { get; set; }
    [Required, MaxLength(100)] public string FirstName { get; set; }
    [Required, MaxLength(100)] public string LastName { get; set; }
    [Required] public DateTime Birthdate { get; set; }
}