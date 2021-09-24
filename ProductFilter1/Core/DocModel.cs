
namespace ProductFilter1
{
	public abstract record DocModel<T> : IDocModel<T>
	{
		public T Id { get; set; }
		public abstract int DocType { get; protected set; }
	}
}
