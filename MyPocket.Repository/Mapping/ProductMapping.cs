using MyPocket.Domain.Product;
using MyPocket.Interfaces.Mapping;

namespace MyPocket.Repository.Mapping
{
    public class ProductMapping : DatabaseMapping<Product>, IProductMapping
    {
        protected override string PathName { get { return "products"; } }
    }
}