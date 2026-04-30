using Domain.Enums;

namespace Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SystemRole Code { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
