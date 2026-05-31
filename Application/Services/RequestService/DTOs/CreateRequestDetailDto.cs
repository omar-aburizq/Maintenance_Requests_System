using Microsoft.AspNetCore.Http;

namespace Application.Services.RequestService.DTOs
{
    public class CreateRequestDetailDto
    {
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public IFormFile? Phot { get; set; }  // Install Microsoft.AspNetCore.Http.Abstractions
    }
}
