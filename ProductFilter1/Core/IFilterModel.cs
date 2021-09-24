
namespace ProductFilter1
{
	public interface IFilterModel<T> : IDocModel<T>
	{
		string Description { get; set; }
	}
}
