using Domain.Enums;

namespace Application.Services.RequestService.DTOs
{
    public class CreateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public Guid EmploeeyId { get; set; }
        public CreateRequestDetailDto RequestDitail { get; set; }
    }

    public class CreateRequestDetailDto
    {
        public string Location { get; set; }
        public string? EmployeeNotes { get; set; }
        public string? ImageURL { get; set; }
    }

}
