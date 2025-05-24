using APBD_11.Data;
using APBD_11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_11.Repositories;

public class PatientsRepository : IPatientsRepository
{
    private readonly DatabaseContext _context;

    public PatientsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetPatientWithDetailsAsync(int idPatient)
    {
        return await _context.Patients.
            Include(p => p.Prescriptions).
                ThenInclude(p => p.PrescriptionMedicaments).
                    ThenInclude(pm => pm.Medicament).
            Include(p => p.Prescriptions).
                ThenInclude(p => p.Doctor).
            FirstOrDefaultAsync(p => p.IdPatient == idPatient);
    }
}