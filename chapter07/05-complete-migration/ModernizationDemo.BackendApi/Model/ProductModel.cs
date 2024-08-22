namespace ModernizationDemo.BackendApi.Model
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
