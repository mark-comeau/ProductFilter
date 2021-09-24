
namespace ProductFilter.Common
{
	public record ProductCategory : DocModel<string>
	{
		public override int DocType { get => (int)CommonCollection.DocType.ProductCategory; protected set { } }
		public string Code { get; set; }
		public string Description { get; set; }
		public ProductCategoryStatus Status { get; set; }
		public bool IsUserDefined { get; set; }
		public string Note { get; set; }
		public string[] Tags { get; set; }
	}
}
