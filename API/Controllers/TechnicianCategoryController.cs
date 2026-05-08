using Application.Services.TechnicianService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianCategoryController : ControllerBase
    {
        private readonly ITechnicianCategoryService _technicianCategoryService;
        public TechnicianCategoryController(ITechnicianCategoryService technicianCategoryService)
        {
            _technicianCategoryService = technicianCategoryService;
        }


    }
}
