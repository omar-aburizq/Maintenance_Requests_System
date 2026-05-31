using Application.Repositories;
using Application.Services.CurrentUserService;
using Application.Services.RequestService.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services.RequestService
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<Request> _requestRepository;
        private readonly IGenericRepository<RequestDetail> _requestDetailRepository;
        private readonly IGenericRepository<RequestHistory> _requestHistoryRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestService(IGenericRepository<Request> requestRepository, IGenericRepository<RequestDetail> requestDetailRepository, IGenericRepository<RequestHistory> requestHistoryRepository, IGenericRepository<User> userRepository, IGenericRepository<Category> categoryRepository, ICurrentUserService currentUserService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _requestRepository = requestRepository;
            _requestDetailRepository = requestDetailRepository;
            _requestHistoryRepository = requestHistoryRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateRequest(CreateRequestDto input)
        {

            if (!(await _categoryRepository.GetAll().AnyAsync(x => x.Id == input.CategoryId)))
                throw new Exception("Category doesn't exist");

            var employeeId = _currentUserService.UserId.Value;

            var data = new Request()
            {
                Id = Guid.NewGuid(),
                Title = input.Title,
                Description = input.Description,
                CreatedAt = DateTime.UtcNow,
                Status = RequestStatus.New,
                CategoryId = input.CategoryId,
                EmployeeId = employeeId,
            };
            await _requestRepository.InsertAsync(data);
            await _requestRepository.SaveChangesAsync();

            // upload attachment

            string? phtoUrl = null;

            if (input.RequestDitails.Phot != null)
                phtoUrl = await UploadImage(input.RequestDitails.Phot);

            var detail = new RequestDetail()
            {
                Id = Guid.NewGuid(),
                RequestId = data.Id,
                Location = input.RequestDitails.Location,
                EmployeeNotes = input.RequestDitails.EmployeeNotes,
                PhotoUrl = phtoUrl
            };
            await _requestDetailRepository.InsertAsync(detail);
            await _requestDetailRepository.SaveChangesAsync();
        }

        // UploadImage Function
        private async Task<string> UploadImage(IFormFile imag)
        {
            var baseUploadPath = _configuration["FileStorage:UploadPath"];
            var UploadsFolder = Path.Combine(baseUploadPath, "Requests");

            if (!Directory.Exists(UploadsFolder))
                Directory.CreateDirectory(UploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imag.FileName);
            var filePath = Path.Combine(UploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
                await imag.CopyToAsync(fileStream);

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/External/Requests/{fileName}";
        }


        public async Task DeleteRequest(Guid id)
        {
            var data = await _requestRepository.GetByIdAsync(id);

            if (data == null)
                throw new Exception("Request not found");

            var detail = await _requestDetailRepository.GetAll().FirstOrDefaultAsync(x => x.RequestId == id);

            if (detail != null)
                _requestDetailRepository.Delete(detail);

            _requestRepository.Delete(data);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task<List<GetAllRequestDto>> GetAllRequest()
        {
            var data = _requestRepository.GetAll().Include(x => x.Category).Include(x => x.Employee).Include(x => x.Technician);

            var result = await data.Select(x => new GetAllRequestDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                Status = x.Status,
                TechnicianName = x.Technician != null ? x.Technician.Name : null,
                EmployeeName = x.Employee.Name,
                CategoryName = x.Category.Name,
            }).OrderBy(x => x.CreatedAt).ToListAsync();

            return result;
        }

        public async Task<GetRequestByIdDto> GetRequestById(Guid id)
        {
            var request = await _requestRepository.GetAll().Include(x => x.Category).Include(x => x.Employee).Include(x => x.Technician).FirstOrDefaultAsync(x => x.Id == id);

            var detail = await _requestDetailRepository.GetAll().FirstOrDefaultAsync(x => x.RequestId == id);

            if (detail == null)
                throw new Exception("Request Detail not found");

            if (request == null)
                throw new Exception("request dosnt Exist");

            var data = new GetRequestByIdDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CreatedAt = request.CreatedAt,
                Status = request.Status,
                TechnicianName = request.Technician != null ? request.Technician.Name : null,
                EmployeeName = request.Employee.Name,
                CategoryName = request.Category.Name,
                GetRequestDetailsById = new GetRequestDetailByIdDto
                {
                    Location = detail.Location,
                    EmployeeNotes = detail.EmployeeNotes,
                    TechnicianNotes = detail.TechnicianNotes,
                    PhotoUrl = detail.PhotoUrl,
                }
            };
            return data;
        }

        public async Task UpdateRequest(Guid id, UpdateRequestDto input)
        {
            if (!(await _categoryRepository.GetAll().AnyAsync(x => x.Id == input.CategoryId)))
                throw new Exception("Category dosent Exist");

            var request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
                throw new Exception("Request not found");

            if (request.Status != RequestStatus.New)
                throw new Exception("Cannot update request after processing started");

            var detail = await _requestDetailRepository.GetAll().FirstOrDefaultAsync(x => x.RequestId == id);

            if (detail == null)
                throw new Exception("Request Detail not found");

            request.Title = input.Title;
            request.Description = input.Description;
            request.CategoryId = input.CategoryId;

            detail.PhotoUrl = input.UpdateRequestDetails.PhotoUrl;
            detail.Location = input.UpdateRequestDetails.Location;
            detail.EmployeeNotes = input.UpdateRequestDetails.EmployeeNotes;

            _requestRepository.Update(request);
            _requestDetailRepository.Update(detail);

            await _requestDetailRepository.SaveChangesAsync();
            await _requestRepository.SaveChangesAsync();
        }

        public async Task AssignTechnician(Guid requestId, Guid technicianId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Request not found");

            if (request.Status == RequestStatus.Cancelled)
                throw new Exception("Cannot assign technician to cancelled request");

            if (request.Status == RequestStatus.Resolved)
                throw new Exception("Cannot assign technician to resolved request");

            var technician = await _userRepository.GetByIdAsync(technicianId);

            if (technician == null)
                throw new Exception("Technician not found");

            request.TechnicianId = technicianId;
            request.Status = RequestStatus.InProgress;

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task ChangeStatus(Guid requestId, RequestStatus status)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Request not found");

            if (request.Status == RequestStatus.Cancelled)
                throw new Exception("Cannot change status for cancelled request");

            if (request.TechnicianId == null)
                throw new Exception("Request must be assigned to technician first");

            request.Status = status;

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

        public async Task CancelRequest(Guid requestId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);

            if (request == null)
                throw new Exception("Request not found");

            if (request.Status != RequestStatus.New)
                throw new Exception("Only new requests can be cancelled");

            request.Status = RequestStatus.Cancelled;

            _requestRepository.Update(request);
            await _requestRepository.SaveChangesAsync();
        }

    }
}
