using System.ComponentModel.DataAnnotations;

namespace APBD_11.DTOs;

public class MedicamentRequestDto
{
    [Required] public int IdMedicament { get; set; }
    public int Dose { get; set; }
    [Required, MaxLength(100)] public string Description { get; set; }
}