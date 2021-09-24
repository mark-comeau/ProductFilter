using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFilter1.Products
{
	public class ProductSearch
	{
		public async Task<Response> ExecuteAsync(IMongoDatabase db, Request criteria)
		{
			var response = new Response();
			var sw = new Stopwatch();
			sw.Restart();
			var productCollection = db.GetCollection<Product>(ProductCollection.Name);
			var builder = Builders<Product>.Filter;
			var filter = builder.Eq(f => f.DocType, (int)ProductCollection.DocType.Product) & builder.Eq(f => f.ProductCategoryId, criteria.ProductCategories.First());
			var result = productCollection.Find(filter);
			response.Products = await result.ToListAsync();
			sw.Stop();
			Console.WriteLine($"ProductSearch.ExecuteAsync: {sw.ElapsedMilliseconds}ms, Product Count = {response.Products.Count}");
			return response;
		}

		public async Task<Response> ExecuteNoIndexHintAsync(IMongoDatabase db, Request criteria)
		{
			var response = new Response();
			var sw = new Stopwatch();
			sw.Restart();
			var productCollection = db.GetCollection<Product>(ProductCollection.Name);
			var builder = Builders<Product>.Filter;
			var filter = builder.Eq(f => f.DocType, (int)ProductCollection.DocType.Product) & builder.Ne(f => f.BrandId, "_") & builder.Eq(f => f.ProductCategoryId, criteria.ProductCategories.First());
			var result = productCollection.Find(filter);
			response.Products = await result.ToListAsync();
			sw.Stop();
			Console.WriteLine($"ProductSearch.ExecuteNoIndexHintAsync: {sw.ElapsedMilliseconds}ms, Product Count = {response.Products.Count}");
			return response;
		}

		public async Task<Response> ExecuteWithIndexHintAsync(IMongoDatabase db, Request criteria)
		{
			var response = new Response();
			var sw = new Stopwatch();
			sw.Restart();
			var productCollection = db.GetCollection<Product>(ProductCollection.Name);
			var builder = Builders<Product>.Filter;
			var filter = builder.Eq(f => f.DocType, (int)ProductCollection.DocType.Product) & builder.Ne(f => f.BrandId, "_") & builder.Ne(f => f.Type, ProductType.None) & builder.Eq(f => f.ProductCategoryId, criteria.ProductCategories.First());
			var result = productCollection.Find(filter, new FindOptions() { Hint = $"{ProductCollection.Name}Index_{nameof(Product.BrandId)}_{nameof(Product.Type)}_{nameof(Product.ProductCategoryId)}_{nameof(Product.Status)}" });
			response.Products = await result.ToListAsync();
			sw.Stop();
			Console.WriteLine($"ProductSearch.ExecuteWithIndexHintAsync: {sw.ElapsedMilliseconds}ms, Product Count = {response.Products.Count}");
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
			public ProductStatus[] Statuses { get; set; }
			public string Note { get; set; }
			public string[] Filters { get; set; }
			public string[] Tags { get; set; }
		}

		public record Response
		{
			public List<Product> Products { get; set; }
		}
	}
}
