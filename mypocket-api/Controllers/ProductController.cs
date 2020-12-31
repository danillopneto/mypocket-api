using Microsoft.AspNetCore.Mvc;
using mypocket.api.Mapping;
using mypocket.api.Models;
using System;
using System.Net;

namespace mypocket.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ApiController<Product>
    {
        public ProductController(IProductMapping db) : base(db)
        {
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <param name="id">Id of the product.</param>
        /// <returns>All products.</returns>
        /// <response code="200">Success by getting the products.</response>
        /// <response code="500">Internal error by getting the list.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public override ActionResult<Product> Get(Guid id)
        {
            return base.Get(id);
        }
    }
}