using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("hello from product api");
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok("products list");
        }
    }
}
