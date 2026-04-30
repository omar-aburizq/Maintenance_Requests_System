namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Location { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<RequestHistory> RequestHistories { get; set; }
        public ICollection<TechnicianCategory> TechnicianCategoryies { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }
}
