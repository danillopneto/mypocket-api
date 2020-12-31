namespace mypocket.api.Mapping
{
    public class ProductMapping : DatabaseMapping, IProductMapping
    {
        protected override string PathName { get { return "products"; } }
    }
}