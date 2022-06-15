using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Category.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("hello from category api");
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok("category list");
        }

        [HttpGet("{id}")]
        public IActionResult GetAllCategories(int id)
        {
            return Ok( $"{id} id'li category detayı...");
        }

    }
}
