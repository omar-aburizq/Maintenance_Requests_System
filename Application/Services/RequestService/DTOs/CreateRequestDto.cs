using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Services.RequestService.DTOs
{
    public class CreateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public CreateRequestDetailDto RequestDitails { get; set; }
    }
}
