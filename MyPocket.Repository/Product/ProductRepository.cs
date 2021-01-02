using MyPocket.Interfaces.Repository;

namespace MyPocket.Repository.Product
{
    public class ProductRepository : BaseRepository<Domain.Product.Product>, IProductRepository
    {
        protected override string PathName { get { return "products"; } }
    }
}