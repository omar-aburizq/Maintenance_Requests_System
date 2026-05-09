using Application.Services.RequestService.DTOs;
using Domain.Enums;

namespace Application.Services.RequestService
{
    public interface IRequestService
    {
        public Task CreateRequest(CreateRequestDto input);
        public Task<List<GetAllRequestDto>> GetAllRequest();
        public Task<GetRequestByIdDto> GetRequestById(Guid id);
        public Task UpdateRequest(Guid id , UpdateRequestDto input );
        public Task DeleteRequest(Guid id);
        public Task AssignTechnician(Guid requestId, Guid technicianId);
        public Task ChangeStatus(Guid requestId, RequestStatus status);
        public Task CancelRequest(Guid requestId);
    }
}
