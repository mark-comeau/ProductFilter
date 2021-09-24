
namespace ProductFilter
{
	public interface IFilterModel<T> : IDocModel<T>
	{
		string Description { get; set; }
	}
}
