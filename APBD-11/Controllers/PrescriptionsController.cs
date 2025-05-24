using APBD_11.DTOs;
using APBD_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionsService _prescriptionsService;

        public PrescriptionsController(IPrescriptionsService prescriptionsService)
        {
            _prescriptionsService = prescriptionsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescriptionAsync(PrescriptionRequestDto prescription)
        {
            int newPrescriptionId = await _prescriptionsService.AddPrescriptionAsync(prescription);
            return StatusCode(201, new { id = newPrescriptionId });
        }
    }
}