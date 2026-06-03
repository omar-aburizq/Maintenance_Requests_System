using Application.Services.RequestService;
using Application.Services.RequestService.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Employee)}")]
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromForm] CreateRequestDto input)
        {
            await _requestService.CreateRequest(input);
            return Ok();
        }

        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var request = await _requestService.GetAllRequest();
            return Ok(request);
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Employee)},{nameof(SystemRole.Technician)}")]
        [HttpGet("GetRequestById")]
        public async Task<IActionResult> GetRequestById(Guid id)
        {
            var request = await _requestService.GetRequestById(id);
            return Ok(request);
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Employee)}")]
        [HttpPut("UpdateRequest")]
        public async Task<IActionResult> UpdateRequest(Guid id, [FromBody] UpdateRequestDto input)
        {
            await _requestService.UpdateRequest(id, input);
            return Ok();
        }


        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpPut("AssignTechnician")]
        public async Task<IActionResult> AssignTechnician(Guid requestId, [FromBody] AssignTechnicianDto input)
        {
            await _requestService.AssignTechnician(requestId, input.TechnicianId);
            return Ok();
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Technician)}")]
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(Guid requestId, [FromBody] ChangeRequestStatusDto input)
        {
            await _requestService.ChangeStatus(requestId, input.Status);
            return Ok();
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Employee)}")]
        [HttpPut("CancelRequest")]
        public async Task<IActionResult> CancelRequest(Guid requestId)
        {
            await _requestService.CancelRequest(requestId);
            return Ok();
        }
    }
}
