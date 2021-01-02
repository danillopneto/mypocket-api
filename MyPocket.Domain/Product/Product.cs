namespace MyPocket.Domain.Product
{
    public class Product : BaseObject
    {
        public string Description { get; set; }

        public double? Price { get; set; }
    }
}