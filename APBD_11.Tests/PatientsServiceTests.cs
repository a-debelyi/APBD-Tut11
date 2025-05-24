using APBD_11.Models;
using APBD_11.Repositories;
using APBD_11.Services;
using APBD_8.Exceptions;
using Moq;

namespace APBD_11.Tests;

public class PatientsServiceTests
{
    private readonly Mock<IPatientsRepository> _mockRepository;
    private readonly PatientsService _service;

    public PatientsServiceTests()
    {
        _mockRepository = new Mock<IPatientsRepository>();
        _service = new PatientsService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetPatientAsync_ShouldReturnPatientWithPrescriptionsWhenPatientExists()
    {
        // Given
        var doctor = new Doctor
        {
            IdDoctor = 1,
            FirstName = "Doctor 1",
            LastName = "1",
            Email = "d1@ex.com"
        };

        var medicament = new Medicament
        {
            IdMedicament = 1,
            Name = "Medicament 1",
            Description = "Very nice",
            Type = "Tablet"
        };

        var prescription = new Prescription
        {
            IdPrescription = 1,
            Date = DateTime.Now,
            DueDate = DateTime.Now.AddDays(10),
            Doctor = doctor,
            IdDoctor = doctor.IdDoctor,
            PrescriptionMedicaments = new List<PrescriptionMedicament>
            {
                new()
                {
                    IdMedicament = 1,
                    Dose = 23,
                    Details = "Description...",
                    Medicament = medicament
                }
            }
        };

        var patient = new Patient
        {
            IdPatient = 1,
            FirstName = "Patient 1",
            LastName = "1",
            Birthdate = new DateTime(2000, 1, 1),
            Prescriptions = new List<Prescription> { prescription }
        };

        _mockRepository.Setup(r => r.GetPatientWithDetailsAsync(patient.IdPatient)).ReturnsAsync(patient);

        // When
        var result = await _service.GetPatientAsync(patient.IdPatient);

        // Then
        Assert.NotNull(result);
        Assert.Equal(patient.IdPatient, result.IdPatient);
        Assert.Single(result.Prescriptions);

        var prescriptionResponseDto = result.Prescriptions.First();
        Assert.Equal(prescription.IdPrescription, prescriptionResponseDto.IdPrescription);
        Assert.Equal(doctor.IdDoctor, prescriptionResponseDto.Doctor.IdDoctor);
        Assert.Single(prescriptionResponseDto.Medicaments);
        Assert.Equal("Medicament 1", prescriptionResponseDto.Medicaments[0].Name);

        _mockRepository.Verify(r => r.GetPatientWithDetailsAsync(patient.IdPatient), Times.Once);
    }

    [Fact]
    public async Task GetPatientAsync_ShouldThrowNotFoundWhenRepoReturnsNull()
    {
        // Given
        int patientId = 1;
        _mockRepository.Setup(r => r.GetPatientWithDetailsAsync(patientId)).ReturnsAsync((Patient?)null);

        // When - Then
        var e = await Assert.ThrowsAsync<NotFoundException>(() => _service.GetPatientAsync(patientId));
        Assert.Contains($"Patient with id {patientId} not found", e.Message);

        _mockRepository.Verify(r => r.GetPatientWithDetailsAsync(patientId), Times.Once);
    }
}