using Application.Services.RequestService;
using Application.Services.RequestService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromForm] CreateRequestDto input)
        {
            await _requestService.CreateRequest(input);
            return Ok();
        }

        [HttpGet("GetAllRequests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var Request = await _requestService.GetAllRequest();
            return Ok(Request);
        }

        [HttpGet("GetRequestById")]
        public async Task<IActionResult> GetRequestById(Guid id)
        {
            var Request = await _requestService.GetRequestById(id);
            return Ok(Request);
        }

        [HttpPut("UpdateRequest")]
        public async Task<IActionResult> UpdateRequest(Guid id, [FromBody] UpdateRequestDto input)
        {
            await _requestService.UpdateRequest(id, input);
            return Ok();
        }

        [HttpDelete("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            await _requestService.DeleteRequest(id);
            return Ok();
        }

        [HttpPut("AssignTechnician")]
        public async Task<IActionResult> AssignTechnician(Guid requestId, [FromBody] AssignTechnicianDto input)
        {
            await _requestService.AssignTechnician(requestId, input.TechnicianId);
            return Ok();
        }

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(Guid requestId, [FromBody] ChangeRequestStatusDto input)
        {
            await _requestService.ChangeStatus(requestId, input.Status);
            return Ok();
        }

        [HttpPut("CancelRequest")]
        public async Task<IActionResult> CancelRequest(Guid requestId)
        {
            await _requestService.CancelRequest(requestId);
            return Ok();
        }
    }
}
