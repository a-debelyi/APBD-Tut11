using APBD_11.Models;

namespace APBD_11.Repositories;

public interface IPatientsRepository
{
    Task<Patient?> GetPatientWithDetailsAsync(int idPatient);
}