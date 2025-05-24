using APBD_11.Data;
using APBD_11.DTOs.Responses;
using APBD_11.Repositories;
using APBD_8.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace APBD_11.Services;

public class PatientsService : IPatientsService
{
    private readonly IPatientsRepository _patientsRepository;

    public PatientsService(IPatientsRepository patientsRepository)
    {
        _patientsRepository = patientsRepository;
    }

    public async Task<PatientResponseDto> GetPatientAsync(int idPatient)
    {
        var patient = await _patientsRepository.GetPatientWithDetailsAsync(idPatient);

        if (null == patient)
            throw new NotFoundException($"Patient with id {idPatient} not found");

        return new PatientResponseDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate).Select(p => new PrescriptionResponseDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentResponseDto
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Description = pm.Details
                }).ToList(),
                Doctor = new DoctorResponseDto
                {
                    IdDoctor = p.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                }
            }).ToList()
        };
    }
}