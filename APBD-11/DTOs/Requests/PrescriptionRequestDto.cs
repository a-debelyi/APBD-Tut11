using System.ComponentModel.DataAnnotations;

namespace APBD_11.DTOs;

public class PrescriptionRequestDto
{
    [Required] public PatientRequestDto Patient { get; set; }
    public List<MedicamentRequestDto> Medicaments { get; set; } = new();
    [Required] public DateTime Date { get; set; }
    [Required] public DateTime DueDate { get; set; }
    [Required] public int IdDoctor { get; set; }
}