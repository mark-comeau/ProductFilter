
namespace ProductFilter1.Products
{
	public record Product : DocModel<string>
	{
		public override int DocType { get => (int)ProductCollection.DocType.Product; protected set { } }
		public string Description { get; set; }
		public string MetricDescription { get; set; }
		public string BrandId { get; set; }
		public ProductType Type { get; set; }
		public string ProductCategoryId { get; set; }
		public ProductStatus Status { get; set; }
		public string Note { get; set; }
		public string[] Filters { get; set; }
		public string[] Tags { get; set; }
	}
}
