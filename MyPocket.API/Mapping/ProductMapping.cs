using MyPocket.Domain.Product;

namespace MyPocket.API.Mapping
{
    public class ProductMapping : DatabaseMapping<Product>, IProductMapping
    {
        protected override string PathName { get { return "products"; } }
    }
}