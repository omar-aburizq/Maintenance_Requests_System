namespace Domain.Entities
{
    public class TechnicianCategory
    {
        public Guid Id { get; set; }

        public Guid TechnicianId { get; set; }
        public User Technician { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
