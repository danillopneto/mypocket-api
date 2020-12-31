using mypocket.api.Models;

namespace mypocket.api.Mapping
{
    public class ProductMapping : DatabaseMapping<Product>, IProductMapping
    {
        protected override string PathName { get { return "products"; } }
    }
}