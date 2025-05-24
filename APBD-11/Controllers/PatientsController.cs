using APBD_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsService _patientsService;

        public PatientsController(IPatientsService patientsService)
        {
            _patientsService = patientsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientAsync(int id)
        {
            var patient = await _patientsService.GetPatientAsync(id);
            return Ok(patient);
        }
    }
}