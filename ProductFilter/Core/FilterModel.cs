
namespace ProductFilter
{
	public abstract record FilterModel<T> : IFilterModel<T>
	{
		public T Id { get; set; }
		public abstract int DocType { get; protected set; }
		public string Description { get; set; }
	}
}
