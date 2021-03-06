
namespace ProductFilter.Common
{
	public record Brand : DocModel<string>
	{
		public override int DocType { get => (int)CommonCollection.DocType.Brand; protected set { } }
		public string Code { get; set; }
		public string Description { get; set; }
		public BrandStatus Status { get; set; }
		public bool IsUserDefined { get; set; }
		public string Note { get; set; }
		public string[] Tags { get; set; }
	}
}
