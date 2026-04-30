namespace Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<Request> Requests { get; set; }
        public ICollection<TechnicianCategory> TechnicianCategoryies { get; set; }
    }
}
