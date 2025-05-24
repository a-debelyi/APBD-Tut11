using APBD_11.DTOs.Responses;

namespace APBD_11.Services;

public interface IPatientsService
{
    Task<PatientResponseDto> GetPatientAsync(int idPatient);
}