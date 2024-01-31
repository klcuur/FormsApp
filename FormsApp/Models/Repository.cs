﻿using Microsoft.VisualBasic;

namespace FormsApp.Models
{
	public class Repository
	{
		private static readonly List<Product> _products = new() ;

		public static List<Product>Products { get { return _products ; } }


		private static readonly List<Category> _categories = new() ;
		public static List<Category> Categories { get { return _categories; } }
		static Repository()
		{
			_categories.Add(new Category { CategoryId = 1, Name = "Telefon" });
			_categories.Add(new Category { CategoryId = 2, Name = "Bilgisayar" });
			
			_products.Add(new Product {  ProductId = 1,Name="IPhone 14",Price=40000, IsActive=true, Image="1.jpg", CategoryId=1});
			_products.Add(new Product { ProductId = 2, Name = "IPhone 15", Price = 50000, IsActive = false, Image = "2.jpg", CategoryId = 1 }); 
			_products.Add(new Product {  ProductId = 3,Name="IPhone 16",Price=60000, IsActive=true, Image="3.jpg", CategoryId=1});
			_products.Add(new Product {  ProductId = 4,Name="IPhone 17",Price=70000, IsActive=false, Image="4.jpg", CategoryId=1});

			_products.Add(new Product {  ProductId = 5,Name="Macbook Air",Price=70000, IsActive=false, Image="9.jpg", CategoryId=2});
			_products.Add(new Product {  ProductId = 6,Name="Macbook Pro",Price=90000, IsActive=true, Image="10.jpg", CategoryId=2});

		}
		public static void CreateProduct(Product entity)
		{
			_products.Add(entity);
		}
		public static void EditProduct(Product updatedProduct)
		{
			var entity= _products.FirstOrDefault(t=>t.ProductId==updatedProduct.ProductId);
			if (entity != null)
			{
				entity.Name = updatedProduct.Name;
				entity.Price  = updatedProduct.Price;
				entity.Image = updatedProduct.Image;
				entity.CategoryId= updatedProduct.CategoryId;
				entity.IsActive= updatedProduct.IsActive;
				
			}
		}
		public static void EditIsActive(Product updatedProduct)
		{
			var entity = _products.FirstOrDefault(t => t.ProductId == updatedProduct.ProductId);
			if (entity != null)
			{
				entity.IsActive = updatedProduct.IsActive;

			}
		}

		public static void DeleteProduct(Product entity)
		{
		 var a = _products.FirstOrDefault(t => t.ProductId == entity.ProductId);
			if (entity != null)
			{
				_products.Remove(a);
			}
		}
			
			
	}
}
