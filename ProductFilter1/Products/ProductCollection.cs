using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ProductFilter1.Products
{
	public static class ProductCollection
	{
		public const string Name = "Product";

		public enum DocType
		{
			Product,
			ProductFilter,
			ProductFilterValue
		}

		public static async Task CreateIndexesAsync(IMongoDatabase db)
		{
			var collection = db.GetCollection<BsonDocument>(Name);
			var indexes = new[] {
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(nameof(Product.DocType)), new CreateIndexOptions() { Name = $"{Name}Index_{nameof(Product.DocType)}" }),
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(nameof(Product.BrandId)).Ascending(nameof(Product.Type)).Ascending(nameof(Product.ProductCategoryId)).Ascending(nameof(Product.Status)), 
					new CreateIndexOptions<BsonDocument> { Name = $"{Name}Index_{nameof(Product.BrandId)}_{nameof(Product.Type)}_{nameof(Product.ProductCategoryId)}_{nameof(Product.Status)}", PartialFilterExpression = Builders<BsonDocument>.Filter.Eq(nameof(Product.DocType), (int)DocType.Product) }),
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(nameof(Product.DocType)).Ascending(nameof(Product.Filters)), new CreateIndexOptions() { Name = $"{Name}Index_{nameof(Product.DocType)}_{nameof(Product.Filters)}" }),
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(nameof(Product.DocType)).Ascending(nameof(Product.Tags)), new CreateIndexOptions() { Name = $"{Name}Index_{nameof(Product.DocType)}_{nameof(Product.Tags)}" }),
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Text(nameof(Product.Description)).Text(nameof(Product.MetricDescription)).Text(nameof(Product.Note)).Text(nameof(Product.Tags)),
					new CreateIndexOptions<BsonDocument> { Name = $"{Name}Index_{nameof(Product.Description)}_{nameof(Product.MetricDescription)}_{nameof(Product.Note)}_{nameof(Product.Tags)}", PartialFilterExpression = Builders<BsonDocument>.Filter.Eq(nameof(Product.DocType), (int)DocType.Product) })
			};
			await collection.Indexes.CreateManyAsync(indexes);
		}
	}
}
