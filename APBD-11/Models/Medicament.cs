using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_11.Models;

[Table("Medicament")]
public class Medicament
{
    [Key] public int IdMedicament { get; set; }

    [Required, MaxLength(100)] public string Name { get; set; }

    [Required, MaxLength(100)] public string Description { get; set; }

    [Required, MaxLength(100)] public string Type { get; set; }

    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } =  new List<PrescriptionMedicament>();
}