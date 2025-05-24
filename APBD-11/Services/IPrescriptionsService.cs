using APBD_11.DTOs;
using APBD_11.DTOs.Responses;
using APBD_11.Models;

namespace APBD_11.Services;

public interface IPrescriptionsService
{
    Task<int> AddPrescriptionAsync(PrescriptionRequestDto prescription);
}