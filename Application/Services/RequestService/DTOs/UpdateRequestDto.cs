using Domain.Enums;

namespace Application.Services.RequestService.DTOs
{
    public class UpdateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public UpdateRequestDetailDto UpdateRequestDetails { get; set; }
    }

    public class UpdateRequestDetailDto
    {
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public string? PhotoURL { get; set; }

    }
}

