using Application.Services.RequestService.DTOs;
using Domain.Enums;

namespace Application.Services.RequestService
{
    public interface IRequestService
    {
        Task CreateRequest(CreateRequestDto input);
        Task<List<GetAllRequestDto>> GetAllRequest();
        Task<GetRequestByIdDto> GetRequestById(Guid id);
        Task UpdateRequest(Guid id, UpdateRequestDto input);
        Task AssignTechnician(Guid requestId, Guid technicianId);
        Task ChangeStatus(Guid requestId, RequestStatus status);
        Task CancelRequest(Guid requestId);
    }
}
