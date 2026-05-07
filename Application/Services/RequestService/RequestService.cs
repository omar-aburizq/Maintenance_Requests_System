using Application.Repositories;
using Application.Services.CurrentUserService;
using Application.Services.RequestService.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Application.Services.RequestService
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<Request> _requestRepository;
        private readonly IGenericRepository<RequestDetail> _requestDetailRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestService(IGenericRepository<Request> requestRepository, IGenericRepository<RequestDetail> requestDetailRepository, IGenericRepository<Category> categoryRepository , IGenericRepository<User> userRepository, ICurrentUserService currentUserService , IConfiguration configuration , IHttpContextAccessor httpContextAccessor)
        { 
            _requestRepository = requestRepository;
            _requestDetailRepository = requestDetailRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
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
                EmploeeyId = employeeId,
            };
            await _requestRepository.InsertAsync(data);
            await _requestRepository.SaveChangesAsync();

            string? phtoUrl = null;

            if (input.CreateRequestDitails.Phto != null)
                phtoUrl = await UploadImage(input.CreateRequestDitails.Phto);

            var detail = new RequestDetail()
            {
                Id = Guid.NewGuid(),
                RequestId = data.Id,
                Location = input.CreateRequestDitails.Location,
                EmployeeNotes = input.CreateRequestDitails.EmployeeNotes ,
                PhotoURL = phtoUrl
            };
            await _requestDetailRepository.InsertAsync(detail);
            await _requestDetailRepository.SaveChangesAsync();
        }

        private async Task<string> UploadImage(IFormFile imag)
        {
            var baseUploadPath = _configuration["FileStorage:UploadPath"];
            var UploadsFolder = Path.Combine(baseUploadPath, "Requests");

            if(!Directory.Exists(UploadsFolder))
                Directory.CreateDirectory(UploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imag.FileName);
            var filePath = Path.Combine(UploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath,FileMode.Create))
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
            var data = _requestRepository.GetAll().Include(x => x.Category).Include(x => x.Emploeey).Include(x => x.Technician);

            var result = await data.Select(x => new GetAllRequestDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                Status = x.Status,
                TechnicianName = x.Technician.Name,
                EmployeeName = x.Emploeey.Name,
                CategoryName = x.Category.Name,
            }).OrderBy(x=> x.CreatedAt).ToListAsync();

            return result;
        }

        public async Task<GetRequestByIdDto> GetRequestById(Guid id)
        {
            var request = await _requestRepository.GetAll().Include(x => x.Category).Include(x => x.Emploeey).Include(x => x.Technician).FirstOrDefaultAsync(x => x.Id == id);

            var detail = await _requestDetailRepository.GetAll().FirstOrDefaultAsync(x => x.RequestId == id);

            if (request == null)
                throw new Exception("request dosnt Exist");

            var data = new GetRequestByIdDto
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CreatedAt = request.CreatedAt,
                Status = request.Status,
                TechnicianName = request.Technician.Name,
                EmployeeName = request.Emploeey.Name,
                CategoryName = request.Category.Name,
                GetRequestDetailsById = new GetRequestDetailByIdDto
                {
                    Location = detail.Location,
                    EmployeeNotes = detail.EmployeeNotes,
                    TechnicianNotes = detail.TechnicianNotes ,
                    PhotoURL = detail.PhotoURL,
                }
            };
            return data;
        }

        public async Task UpdateRequest(Guid id, UpdateRequestDto input)
        {
            if (!(await _categoryRepository.GetAll().AnyAsync(x => x.Id == input.CategoryId)))
                throw new Exception("Category dosent Exist");

            var data = await _requestRepository.GetByIdAsync(id);

            if (data == null)
                throw new Exception("Request not found");

            var detail = await _requestDetailRepository.GetAll().FirstOrDefaultAsync(x => x.RequestId == id);

            if (detail == null)
                throw new Exception("Request Detail not found");

            data.Title = input.Title;
            data.Description = input.Description;
            data.CategoryId = input.CategoryId;

            detail.PhotoURL = input.UpdateRequestDetails.PhotoURL;
            detail.Location = input.UpdateRequestDetails.Location;
            detail.EmployeeNotes = input.UpdateRequestDetails.EmployeeNotes;

            _requestRepository.Update(data);
            _requestDetailRepository.Update(detail);

            await _requestDetailRepository.SaveChangesAsync();
            await _requestRepository.SaveChangesAsync();
        }


    }
}
