using Microsoft.AspNetCore.Mvc;
using ISC_Project.Services;
using ISC_Project.DTOs;
using ISC_Project.DTOs.AcceptingSchoolTransfer;
using ISC_Project.Interface;
using Microsoft.AspNetCore.Authorization;

namespace ISC_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AcceptingSchoolTransfersController : ControllerBase
    {
        private readonly IAcceptingSchoolTransferService _service;

        public AcceptingSchoolTransfersController(IAcceptingSchoolTransferService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AcceptingSchoolTransferDto>>> GetAcceptingSchoolTransfers()
        {
            var transfers = await _service.GetAllAsync();
            return Ok(transfers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AcceptingSchoolTransferDto>> GetAcceptingSchoolTransfer(int id)
        {
            var transfer = await _service.GetByIdAsync(id);

            if (transfer == null)
            {
                return NotFound();
            }

            return Ok(transfer);
        }

        [HttpPost]
        public async Task<ActionResult<AcceptingSchoolTransferDto>> CreateAcceptingSchoolTransfer(CreateAcceptingSchoolTransferDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTransfer = await _service.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetAcceptingSchoolTransfer), new { id = newTransfer.AcceptingSchoolTransfersId }, newTransfer);
        }
    }
}