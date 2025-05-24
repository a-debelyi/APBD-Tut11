using APBD_11.Models;

namespace APBD_11.Repositories;

public interface IPrescriptionsRepository
{
    Task<Medicament?> GetMedicamentAsync(int idMedicament);
    Task<Doctor?> GetDoctorAsync(int idDoctor);
    Task<Patient?> GetPatientAsync(int idPatient);
    Task AddPatientAsync(Patient patient);
    Task<int> AddPrescriptionAsync(Prescription prescription);
    Task AddPrescriptionMedicamentsAsync(IEnumerable<PrescriptionMedicament> items);
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}