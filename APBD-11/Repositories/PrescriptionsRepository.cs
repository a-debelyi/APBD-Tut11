using APBD_11.Data;
using APBD_11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace APBD_11.Repositories;

public class PrescriptionsRepository : IPrescriptionsRepository
{
    private readonly DatabaseContext _context;
    private IDbContextTransaction? _transaction;


    public PrescriptionsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Medicament?> GetMedicamentAsync(int idMedicament)
    {
        return await _context.Medicaments.FirstOrDefaultAsync(m => m.IdMedicament == idMedicament);
    }

    public async Task<Doctor?> GetDoctorAsync(int idDoctor)
    {
        return await _context.Doctors.FirstOrDefaultAsync(doctor => doctor.IdDoctor == idDoctor);
    }

    public async Task<Patient?> GetPatientAsync(int idPatient)
    {
        return await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == idPatient);
    }

    public async Task AddPatientAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<int> AddPrescriptionAsync(Prescription prescription)
    {
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
        return prescription.IdPrescription;
    }

    public async Task AddPrescriptionMedicamentsAsync(IEnumerable<PrescriptionMedicament> items)
    {
        await _context.PrescriptionMedicaments.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (null != _transaction)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (null != _transaction)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}