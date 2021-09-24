using Bogus;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using ProductFilter.Common;
using ProductFilter.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFilter
{
	public class Program
	{
		private static List<Brand> Brands;
		private static List<ProductCategory> ProductCategories;

		public static async Task Main(string[] args)
		{
			string connectionString = "mongodb://localhost:27017?appName=ProductFilter&retryWrites=true&w=majority";
			ConventionRegistry.Register(nameof(IgnoreIfNullConvention), new ConventionPack { new IgnoreIfNullConvention(true) }, t => true);
			BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
			BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
			var client = new MongoClient(MongoClientSettings.FromConnectionString(connectionString));
			var db = client.GetDatabase("ProductFilter");
			//delete database
			if (db != null)
				await client.DropDatabaseAsync("ProductFilter");
			db = client.GetDatabase("ProductFilter");

			//Load from json files
			await ImportCommonCollectionDataAsync(db);
			await ImportProductCollectionDataAsync(db);

			//Create from code
			//await CreateCommonCollectionDataAsync(db);
			//await CreateProductCollectionDataAsync(db);


			var productSearch = new ProductSearch();
			var result = await productSearch.ExecuteAsync(db, new ProductSearch.Request { ProductCategories = new[] { "1_2" } });
			Console.WriteLine($"Done: Count = {result.Products.Count}");
			Console.ReadKey();
		}

		private static async Task ImportCommonCollectionDataAsync(IMongoDatabase db)
		{
			Brands = BsonSerializer.Deserialize<List<Brand>>(File.ReadAllText("Data\\Common.Brand.json"));
			var brandCollection = db.GetCollection<Brand>(CommonCollection.Name);
			await brandCollection.InsertManyAsync(Brands);

			ProductCategories = BsonSerializer.Deserialize<List<ProductCategory>>(File.ReadAllText("Data\\Common.ProductCategory.json"));
			var productCategoryCollection = db.GetCollection<ProductCategory>(CommonCollection.Name);
			await productCategoryCollection.InsertManyAsync(ProductCategories);

			await CommonCollection.CreateIndexesAsync(db);
		}

		private static async Task ImportProductCollectionDataAsync(IMongoDatabase db)
		{
			var productFilters = BsonSerializer.Deserialize<List<Products.ProductFilter>>(File.ReadAllText("Data\\Product.ProductFilter.json"));
			var productFilterCollection = db.GetCollection<Products.ProductFilter>(ProductCollection.Name);
			await productFilterCollection.InsertManyAsync(productFilters);

			var products = BsonSerializer.Deserialize<List<Product>>(File.ReadAllText("Data\\Product.Product.json"));
			var productCollection = db.GetCollection<Product>(ProductCollection.Name);
			await productCollection.InsertManyAsync(products);

			await ProductCollection.CreateIndexesAsync(db);
		}

		private static async Task CreateCommonCollectionDataAsync(IMongoDatabase db)
		{
			Brands = new List<Brand> {
				new Brand
				{
					Id = $"{(int)CommonCollection.DocType.Brand}_1",
					Code = "BR1",
					Description = "Brand 1",
					Status = BrandStatus.Active,
					IsUserDefined = false,
					Note = "This is Brand 1",
					Tags = new[] { "Doors", "Frames" }
				},
				new Brand
				{
					Id = $"{(int)CommonCollection.DocType.Brand}_2",
					Code = "BR2",
					Description = "Brand 2",
					Status = BrandStatus.Active,
					IsUserDefined = false,
					Note = "This is Brand 2",
					Tags = new[] { "Hardware" }
				},
				new Brand
				{
					Id = $"{(int)CommonCollection.DocType.Brand}_3",
					Code = "BR3",
					Description = "Brand 3",
					Status = BrandStatus.Active,
					IsUserDefined = false,
					Note = "This is Brand 3",
					Tags = new[] { "Europe" }
				},
				new Brand
				{
					Id = $"{(int)CommonCollection.DocType.Brand}_4",
					Code = "BR4",
					Description = "Brand 4",
					Status = BrandStatus.Active,
					IsUserDefined = false,
					Note = "This is Brand 4",
					Tags = new[] { "North America" }
				},
				new Brand
				{
					Id = $"{(int)CommonCollection.DocType.Brand}_5",
					Code = "BR5",
					Description = "Brand 5",
					Status = BrandStatus.Inactive,
					IsUserDefined = false,
					Note = "This is Brand 5 is inactive",
					Tags = new[] { "Canada", "USA" }
				},
				new Brand
				{
					Id = $"{(int)CommonCollection.DocType.Brand}U_1",
					Code = "BRU1",
					Description = "User Brand 1",
					Status = BrandStatus.Active,
					IsUserDefined = true,
					Note = "This is a user defined Brand",
					Tags = new[] { "Doors", "Frames", "Hardware" }
				}
			};
			var brandCollection = db.GetCollection<Brand>(CommonCollection.Name);
			await brandCollection.InsertManyAsync(Brands);

			ProductCategories = new List<ProductCategory> {
				new ProductCategory
				{
					Id = $"{(int)CommonCollection.DocType.ProductCategory}_1",
					Code = "PC1",
					Description = "Product Category 1",
					Status = ProductCategoryStatus.Active,
					IsUserDefined = false,
					Note = "This is Product Category 1",
					Tags = null
				},
				new ProductCategory
				{
					Id = $"{(int)CommonCollection.DocType.ProductCategory}_2",
					Code = "PC2",
					Description = "Product Category 2",
					Status = ProductCategoryStatus.Active,
					IsUserDefined = false,
					Note = "This is Product Category 2",
					Tags = new[] { "Key", "Lock" }
				},
				new ProductCategory
				{
					Id = $"{(int)CommonCollection.DocType.ProductCategory}_3",
					Code = "PC3",
					Description = "Product Category 3",
					Status = ProductCategoryStatus.Active,
					IsUserDefined = false,
					Note = null,
					Tags = new[] { "Closer", "Key", "Lock" }
				},
				new ProductCategory
				{
					Id = $"{(int)CommonCollection.DocType.ProductCategory}_4",
					Code = "PC4",
					Description = "Product Category 4",
					Status = ProductCategoryStatus.Inactive,
					IsUserDefined = false,
					Note = "Product Category 4 is inactive",
					Tags = null
				},
				new ProductCategory
				{
					Id = $"{(int)CommonCollection.DocType.ProductCategory}U_1",
					Code = "PCU1",
					Description = "User Product Category",
					Status = ProductCategoryStatus.Active,
					IsUserDefined = true,
					Note = "User Defined Product Category 1",
					Tags = new[] { "Closer", "Key" }
				}
			};
			var productCategoryCollection = db.GetCollection<ProductCategory>(CommonCollection.Name);
			await productCategoryCollection.InsertManyAsync(ProductCategories);

			await CommonCollection.CreateIndexesAsync(db);
		}

		private static async Task CreateProductCollectionDataAsync(IMongoDatabase db)
		{
			var productFilters = Enumerable.Range(1, 1000).Select(s => new Products.ProductFilter() { Id = $"{(int)ProductCollection.DocType.ProductFilter}_{s}", Description = $"Product Filter Description {s}" }).ToList();
			var productFilterCollection = db.GetCollection<Products.ProductFilter>(ProductCollection.Name);
			await productFilterCollection.InsertManyAsync(productFilters);

			var productCollection = db.GetCollection<Product>(ProductCollection.Name);
			int count = 5;
			int batchSize = 1000;
			for (int i = 0; i < count; i++)
			{
				int id = (i * batchSize) + 1;
				var products = new Faker<Product>()
					.StrictMode(true)
					.RuleFor(p => p.Id, f => $"{(int)ProductCollection.DocType.Product}_{id++}")
					.RuleFor(p => p.DocType, f => (int)ProductCollection.DocType.Product)
					.RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
					.RuleFor(p => p.MetricDescription, f => f.Commerce.ProductDescription())
					.RuleFor(p => p.BrandId, f => f.PickRandom(Brands).Id)
					.RuleFor(p => p.Type, f => f.PickRandom<ProductType>())
					.RuleFor(p => p.ProductCategoryId, f => f.PickRandom(ProductCategories).Id)
					.RuleFor(p => p.Status, f => f.PickRandom<ProductStatus>())
					.RuleFor(p => p.Note, f =>
					{
						string result = null;
						var words = f.Lorem.Words(f.Random.Byte(0, 10));
						if (words != null && words.Length > 0)
							result = string.Join(" ", words);
						return result;
					})
					.RuleFor(p => p.Filters, f =>
					{
						List<KeyValuePair<string, object>> result = null;
						var filters = f.Make(f.Random.Byte(0, 20), a => f.PickRandom(productFilters)).Distinct().Select(s => new KeyValuePair<string, object>(s.Id, f.Commerce.ProductName())).ToList();
						if (filters != null && filters.Count > 0)
							result = filters;
						return result;
					})
					.RuleFor(p => p.Tags, f =>
					{
						string[] result = null;
						var tags = f.Lorem.Words(f.Random.Byte(0, 25)).Distinct().ToArray();
						if (tags != null && tags.Length > 0)
							result = tags;
						return result;
					});
				var result = products.Generate(batchSize);
				await productCollection.InsertManyAsync(result);
			}

			await ProductCollection.CreateIndexesAsync(db);
		}
	}
}
