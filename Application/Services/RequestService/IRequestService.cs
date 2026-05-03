using Application.Services.RequestService.DTOs;

namespace Application.Services.RequestService
{
    public interface IRequestService
    {
        public Task CreateRequest(CreateRequestDto input);
        public Task<List<GetAllRequestDto>> GetAllRequest();
        public Task<GetRequestByIdDto> GetRequestById(Guid id);
        public Task UpdateRequest(Guid id , UpdateRequestDto input );
        public Task DeleteRequest(Guid id);
    }
}
