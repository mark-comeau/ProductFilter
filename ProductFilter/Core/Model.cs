
namespace ProductFilter
{
	public abstract record Model<T> : IModel<T>
	{
		public T Id { get; set; }
	}
}
