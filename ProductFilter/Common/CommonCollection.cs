using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ProductFilter.Common
{
	public static class CommonCollection
	{
		public const string Name = "Common";

		public enum DocType
		{
			Brand,
			ProductCategory
		}

		public static async Task CreateIndexesAsync(IMongoDatabase db)
		{
			var collection = db.GetCollection<BsonDocument>(Name);
			var indexes = new[] {
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(nameof(Brand.DocType)), new CreateIndexOptions() { Name = $"{Name}Index_{nameof(Brand.DocType)}" }),
				new CreateIndexModel<BsonDocument>(Builders<BsonDocument>.IndexKeys.Ascending(nameof(Brand.DocType)).Ascending(nameof(Brand.Tags)), new CreateIndexOptions() { Name = $"{Name}Index_{nameof(Brand.DocType)}_{nameof(Brand.Tags)}" })
				};
			await collection.Indexes.CreateManyAsync(indexes);
		}
	}
}
