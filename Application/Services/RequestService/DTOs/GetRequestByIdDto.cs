using Domain.Entities;
using Domain.Enums;

namespace Application.Services.RequestService.DTOs
{
    public class GetRequestByIdDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public RequestStatus Status { get; set; }
        public string CategoryName { get; set; }
        public string EmployeeName { get; set; }
        public string TechnicianName { get; set; }
        public GetRequestDetailByIdDto GetRequestDetailsById {  get; set; }
    }

    public class GetRequestDetailByIdDto
    {
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public string TechnicianNotes { get; set; }
        public string? PhotoURL { get; set; }

    }
}
