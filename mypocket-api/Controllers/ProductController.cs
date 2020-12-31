using Microsoft.AspNetCore.Mvc;
using mypocket.api.Models;

namespace mypocket.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ApiController<Product>
    {
        protected override string PathName { get { return "products"; } }
    }
}