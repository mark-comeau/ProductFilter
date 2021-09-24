
namespace ProductFilter1
{
	public abstract record Model<T> : IModel<T>
	{
		public T Id { get; set; }
	}
}
