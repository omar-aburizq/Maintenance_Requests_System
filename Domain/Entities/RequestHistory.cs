using Domain.Enums;

namespace Domain.Entities
{
    public class RequestHistory
    {
        public Guid Id { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public RequestStatus? OldStatus { get; set; }
        public RequestStatus NewStatus { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RequestId { get; set; }
        public Request Request { get; set; }
    }
}
