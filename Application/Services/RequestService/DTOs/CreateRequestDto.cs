using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.RequestService.DTOs
{
    public class CreateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public CreateRequestDetailDto CreateRequestDitails { get; set; }
    }

    public class CreateRequestDetailDto
    {
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public IFormFile? Phto { get; set; } // Install Microsoft.AspNetCore.Http.Abstractions
    }

}
