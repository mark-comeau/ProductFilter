
namespace ProductFilter1.Products
{
	public record ProductFilterValue : DocModel<string>
	{
		public override int DocType { get => (int)ProductCollection.DocType.ProductFilterValue; protected set { } }
		public string ProductFilterId { get; set; }
		public object Value { get; set; }
	}
}
