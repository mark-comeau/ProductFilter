
namespace ProductFilter.Products
{
	public record ProductFilter : FilterModel<string>
	{
		public ProductFilter()
		{ }

		public ProductFilter(string id) => Id = id;

		public ProductFilter(string id, string description) : this(id) => Description = description;

		public override int DocType { get => (int)ProductCollection.DocType.ProductFilter; protected set { } }
	}
}
