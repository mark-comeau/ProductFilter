using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFilter.Products
{
	public class ProductSearch
	{
		public async Task<Response> ExecuteAsync(IMongoDatabase db, Request criteria)
		{
			var response = new Response();
			var productCollection = db.GetCollection<Product>(ProductCollection.Name);
			var builder = Builders<Product>.Filter;
			var filter = builder.Eq(f => f.DocType, (int)ProductCollection.DocType.Product) & builder.Eq(f => f.ProductCategoryId, criteria.ProductCategories.First());
			var query = productCollection.Find(filter);
			response.Products = await query.ToListAsync();
			return response;
		}

		public record Request
		{
			public string FullTextSearch { get; set; }
			public string Description { get; set; }
			public string MetricDescription { get; set; }
			public string[] Brands { get; set; }
			public ProductType[] Types { get; set; }
			public string[] ProductCategories { get; set; }
			public ProductStatus? Statuses { get; set; }
			public string Note { get; set; }
			public List<KeyValuePair<string, object>> Filters { get; set; }
			public string[] Tags { get; set; }
		}

		public record Response
		{
			public List<Product> Products { get; set; }
		}
	}
}
