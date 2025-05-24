using APBD_11.DTOs;
using APBD_11.Models;
using APBD_11.Repositories;
using APBD_8.Exceptions;

namespace APBD_11.Services;

public class PrescriptionsService : IPrescriptionsService
{
    private readonly IPrescriptionsRepository _prescriptionsRepository;

    public PrescriptionsService(IPrescriptionsRepository prescriptionsRepository)
    {
        this._prescriptionsRepository = prescriptionsRepository;
    }

    public async Task<int> AddPrescriptionAsync(PrescriptionRequestDto prescription)
    {
        if (prescription.Medicaments.Count > 10)
            throw new BadRequestException("A prescription can include a maximum of 10 medications");
        if (prescription.DueDate < prescription.Date)
            throw new BadRequestException("A due date cannot be earlier than a prescription date");

        await _prescriptionsRepository.BeginTransactionAsync();
        try
        {
            foreach (var medicament in prescription.Medicaments)
            {
                var medication = await _prescriptionsRepository.GetMedicamentAsync(medicament.IdMedicament);
                if (null == medication)
                    throw new NotFoundException($"Medicament with id {medicament.IdMedicament} not found");
            }

            var doctor = await _prescriptionsRepository.GetDoctorAsync(prescription.IdDoctor);
            if (null == doctor)
                throw new NotFoundException($"Doctor with id {prescription.IdDoctor} not found");

            var patient = await _prescriptionsRepository.GetPatientAsync(prescription.Patient.IdPatient);
            if (null == patient)
            {
                patient = new Patient
                {
                    FirstName = prescription.Patient.FirstName,
                    LastName = prescription.Patient.LastName,
                    Birthdate = prescription.Patient.Birthdate,
                };
                await _prescriptionsRepository.AddPatientAsync(patient);
            }

            var newPrescription = new Prescription
            {
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = prescription.IdDoctor,
            };
            var newPrescriptionId = await _prescriptionsRepository.AddPrescriptionAsync(newPrescription);

            var prescriptionMedicaments = prescription.Medicaments.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                IdPrescription = newPrescription.IdPrescription,
                Dose = m.Dose,
                Details = m.Description
            }).ToList();
            await _prescriptionsRepository.AddPrescriptionMedicamentsAsync(prescriptionMedicaments);

            await _prescriptionsRepository.CommitAsync();
            return newPrescriptionId;
        }
        catch
        {
            await _prescriptionsRepository.RollbackAsync();
            throw;
        }
    }
}