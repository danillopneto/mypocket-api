using Microsoft.AspNetCore.Mvc;
using mypocket.api.Mapping;
using mypocket.api.Models;

namespace mypocket.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ApiController<Product>
    {
        public ProductController(IProductMapping db) : base(db)
        {
        }
    }
}