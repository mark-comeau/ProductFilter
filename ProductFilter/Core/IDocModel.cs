
namespace ProductFilter
{
	public interface IDocModel<T> : IModel<T>
	{
		int DocType { get; }
	}
}
