
namespace ProductFilter1
{
	public interface IDocModel<T> : IModel<T>
	{
		int DocType { get; }
	}
}
