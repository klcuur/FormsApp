using FormsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IO;

namespace FormsApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index(string searchString, string category)
		{
			var products = Repository.Products;
			if (!String.IsNullOrEmpty(searchString))
			{
				ViewBag.SearchString = searchString;
				products = products.Where(t => t.Name.ToLower().Contains(searchString)).ToList();
			}
			if (!String.IsNullOrEmpty(category) && category != "0")
			{
				products = products.Where(t => t.CategoryId == int.Parse(category)).ToList();
			}
			//ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name",category);
			var model = new ProductViewModel
			{
				Products = products,
				Categories = Repository.Categories,
				SelectedCategory = category
			};
			return View(model);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Product model, IFormFile imageFile)
		{

			var extension = "";


			if (imageFile != null)
			{
				var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
				extension = Path.GetExtension(imageFile.FileName); // abc.jpg
				if (!allowedExtensions.Contains(extension))
				{
					ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
				}
			}

			if (ModelState.IsValid)
			{
				
				var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
				
				using (var stream = new FileStream(path, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}
				model.Image = randomFileName;
				model.ProductId = Repository.Products.Count + 1;
				Repository.CreateProduct(model);
				return RedirectToAction("Index");
			}
			ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
			return View(model);
		}

		public IActionResult Edit(int? id)

		{
			if (id == null)
			{
				return RedirectToAction(nameof(Index), "Home");
			}
			var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
			if (entity == null)
			{
				return RedirectToAction(nameof(Index), "Home");
			}
			
			ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
			return View(entity);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, Product model, IFormFile? imageFile)

		{
			if (id != model.ProductId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				
				if (imageFile != null)
				{
					var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
					var extension = Path.GetExtension(imageFile.FileName); // abc.jpg
					var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						await imageFile.CopyToAsync(stream);
					}
					model.Image = randomFileName;

				}
				
				Repository.EditProduct(model);
				return RedirectToAction(nameof(Index), "Home");
			}

			ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
			return View(model);

		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var entity = Repository.Products.FirstOrDefault(p => p.ProductId == id);
			if(entity == null)
			{
				return NotFound();
			}
			
			return View("DeleteConfirm",entity);
		}
		[HttpPost]
		public IActionResult Delete(int id, int ProductId)
		{
			if(id!=ProductId)
			{
				return NotFound();
			}
			var entity=Repository.Products.FirstOrDefault(p=>p.ProductId == ProductId);
			if(entity == null)
			{
				return NotFound();
			}
			Repository.DeleteProduct(entity);
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		public IActionResult EditProducts(List<Product> products)
		{
			foreach(var product in products)
			{
				Repository.EditIsActive(product);
			}
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}