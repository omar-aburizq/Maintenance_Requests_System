namespace Application.Services.CategoryService.DTOs
{
    public class GetCategoryByIdDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
