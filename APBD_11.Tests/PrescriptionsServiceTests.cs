using APBD_11.DTOs;
using APBD_11.Models;
using APBD_11.Repositories;
using APBD_11.Services;
using APBD_8.Exceptions;
using Moq;

namespace APBD_11.Tests;

public class PrescriptionsServiceTests
{
    private readonly Mock<IPrescriptionsRepository> _mockRepository;
    private readonly PrescriptionsService _service;

    public PrescriptionsServiceTests()
    {
        _mockRepository = new Mock<IPrescriptionsRepository>();
        _service = new PrescriptionsService(_mockRepository.Object);
    }

    [Fact]
    public async Task AddPrescriptionAsync_ShouldThrowBadRequestWhenMoreThan10Medicaments()
    {
        // Given
        var prescriptionRequestDto = new PrescriptionRequestDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(10),
            IdDoctor = 1,
            Patient = new PatientRequestDto(),
            Medicaments = Enumerable.Range(1, 11).Select(i => new MedicamentRequestDto
                { IdMedicament = i, Dose = 1, Description = "D" }).ToList()
        };

        // When - Then
        var e = await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.AddPrescriptionAsync(prescriptionRequestDto));
        Assert.Equal("A prescription can include a maximum of 10 medications", e.Message);

        _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AddPrescriptionAsync_ShouldThrowBadRequestWhenDueDateBeforeDate()
    {
        // Given
        var prescriptionRequestDto = new PrescriptionRequestDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(-10),
            IdDoctor = 1,
            Patient = new PatientRequestDto(),
            Medicaments = new List<MedicamentRequestDto>()
        };

        // When - Then
        var e = await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.AddPrescriptionAsync(prescriptionRequestDto));
        Assert.Equal("A due date cannot be earlier than a prescription date", e.Message);

        _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AddPrescriptionAsync_ShouldThrowNotFoundWhenMedicamentNotFound()
    {
        // Given
        int idMedicament = 101;
        var prescriptionRequestDto = new PrescriptionRequestDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(10),
            IdDoctor = 1,
            Patient = new PatientRequestDto(),
            Medicaments = new List<MedicamentRequestDto>
                { new() { IdMedicament = idMedicament, Dose = 1, Description = "..." } }
        };

        _mockRepository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.GetMedicamentAsync(idMedicament)).ReturnsAsync((Medicament?)null);
        _mockRepository.Setup(r => r.RollbackAsync()).Returns(Task.CompletedTask);

        // When - Then
        var e = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.AddPrescriptionAsync(prescriptionRequestDto));
        Assert.Equal($"Medicament with id {idMedicament} not found", e.Message);

        _mockRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
        _mockRepository.Verify(r => r.GetMedicamentAsync(idMedicament), Times.Once);
        _mockRepository.Verify(r => r.RollbackAsync(), Times.Once);
        _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AddPrescriptionAsync_ShouldThrowNotFoundWhenDoctorNotFound()
    {
        // Given
        int idDoctor = 101;
        var prescriptionRequestDto = new PrescriptionRequestDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(10),
            IdDoctor = idDoctor,
            Patient = new PatientRequestDto(),
            Medicaments = new List<MedicamentRequestDto>()
        };

        _mockRepository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.GetDoctorAsync(idDoctor)).ReturnsAsync((Doctor?)null);
        _mockRepository.Setup(r => r.RollbackAsync()).Returns(Task.CompletedTask);

        // When - Then
        var e = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.AddPrescriptionAsync(prescriptionRequestDto));
        Assert.Equal($"Doctor with id {idDoctor} not found", e.Message);

        _mockRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
        _mockRepository.Verify(r => r.GetDoctorAsync(idDoctor), Times.Once);
        _mockRepository.Verify(r => r.RollbackAsync(), Times.Once);
        _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AddPrescriptionAsync_ShouldCreatePrescriptionAndPatientWhenPatientNotExists()
    {
        // Given
        int idPatient = 101;
        int idDoctor = 1;
        int idMedicament = 1;
        int idPrescription = 101;
        
        var prescriptionRequestDto = new PrescriptionRequestDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(10),
            IdDoctor = idDoctor,
            Patient = new PatientRequestDto
            {
                IdPatient = 0,
                FirstName = "Patient 1",
                LastName = "1",
                Birthdate = DateTime.Today
            },
            Medicaments = new List<MedicamentRequestDto> { new() { IdMedicament = 1, Dose = 2, Description = "D" } }
        };
        
        _mockRepository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.GetMedicamentAsync(idMedicament)).ReturnsAsync(new Medicament { IdMedicament = idMedicament });
        _mockRepository.Setup(r => r.GetDoctorAsync(idDoctor)).ReturnsAsync(new Doctor { IdDoctor = idDoctor });
        _mockRepository.Setup(r => r.GetPatientAsync(0)).ReturnsAsync((Patient?)null);
        _mockRepository.Setup(r => r.AddPatientAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask)
            .Callback<Patient>(p => p.IdPatient = idPatient);
        _mockRepository.Setup(r => r.AddPrescriptionAsync(It.IsAny<Prescription>())).ReturnsAsync(idPrescription);
        _mockRepository.Setup(r => r.AddPrescriptionMedicamentsAsync(It.IsAny<IEnumerable<PrescriptionMedicament>>()))
            .Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.CommitAsync()).Returns(Task.CompletedTask);

        // When
        var result = await _service.AddPrescriptionAsync(prescriptionRequestDto);

        // Then
        Assert.Equal(idPrescription, result);
        _mockRepository.Verify(r => r.BeginTransactionAsync(), Times.Once);
        _mockRepository.Verify(r => r.GetMedicamentAsync(idMedicament), Times.Once);
        _mockRepository.Verify(r => r.GetDoctorAsync(idDoctor), Times.Once);
        _mockRepository.Verify(r => r.GetPatientAsync(0), Times.Once);
        _mockRepository.Verify(
            r => r.AddPatientAsync(It.Is<Patient>(p => p.FirstName == "Patient 1" && p.IdPatient == idPatient)),
            Times.Once);
        _mockRepository.Verify(r => r.AddPrescriptionAsync(It.Is<Prescription>(pr =>
            pr.Date == prescriptionRequestDto.Date && pr.DueDate == prescriptionRequestDto.DueDate &&
            pr.IdPatient == idPatient)), Times.Once);
        _mockRepository.Verify(r => r.AddPrescriptionMedicamentsAsync(It.IsAny<IEnumerable<PrescriptionMedicament>>()),
            Times.Once);
        _mockRepository.Verify(r => r.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPrescriptionAsync_ShouldCreatePrescriptionWhenPatientExists()
    {
        // Given
        int idPatient = 1;
        int idDoctor = 1;
        int idPrescription = 1;

        var prescriptionRequestDto = new PrescriptionRequestDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(10),
            IdDoctor = idDoctor,
            Patient = new PatientRequestDto { IdPatient = 1 },
            Medicaments = new List<MedicamentRequestDto>()
        };

        _mockRepository.Setup(r => r.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.GetDoctorAsync(idDoctor)).ReturnsAsync(new Doctor { IdDoctor = idDoctor });
        _mockRepository.Setup(r => r.GetPatientAsync(idPatient)).ReturnsAsync(new Patient { IdPatient = idPatient });
        _mockRepository.Setup(r => r.AddPrescriptionAsync(It.IsAny<Prescription>())).ReturnsAsync(idPrescription);
        _mockRepository.Setup(r => r.AddPrescriptionMedicamentsAsync(It.IsAny<IEnumerable<PrescriptionMedicament>>()))
            .Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.CommitAsync()).Returns(Task.CompletedTask);

        // When
        var result = await _service.AddPrescriptionAsync(prescriptionRequestDto);

        // Then
        Assert.Equal(idPrescription, result);
        _mockRepository.Verify(r => r.GetPatientAsync(idPatient), Times.Once);
        _mockRepository.Verify(r => r.AddPatientAsync(It.IsAny<Patient>()), Times.Never);
        _mockRepository.Verify(r => r.AddPrescriptionAsync(It.Is<Prescription>(pr =>
            pr.IdPatient == idPatient && pr.IdDoctor == idDoctor)), Times.Once);
        _mockRepository.Verify(r => r.CommitAsync(), Times.Once);
    }
}