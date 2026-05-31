using Domain.Enums;

namespace Domain.Entities
{
    public class Request
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.New;

        public Guid EmployeeId { get; set; } // Created By
        public User Employee { get; set; }
        public Guid? TechnicianId { get; set; }  // Assigned 
        public User? Technician { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<RequestHistory> RequestHistories { get; set; }
    }
}
