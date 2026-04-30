using Microsoft.EntityFrameworkCore;
namespace Domain.Entities
{
    [Index(nameof(RequestId), IsUnique = true)]
    public class RequestDetail
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string EmployeeNotes { get; set; }
        public string TechnicianNotes { get; set; }
        public string? PhotoURL { get; set; }

        public Guid RequestId { get; set; }
        public Request Request { get; set; }
    }
}
