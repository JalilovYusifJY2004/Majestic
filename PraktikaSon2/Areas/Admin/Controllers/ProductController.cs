using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraktikaSon2.Areas.Admin.ViewModels;
using PraktikaSon2.DAL;
using PraktikaSon2.Models;
using PraktikaSon2.Utilities.Extension;

namespace PraktikaSon2.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

		public ProductController(AppDbContext context,IWebHostEnvironment env)
		{
			_context = context;
		_env = env;
		}
		public async Task<IActionResult> Index()
		{
			List<Product> products = await _context.Products.ToListAsync();
			return View(products);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateProductVM createProduct)
		{
			if (!ModelState.IsValid)
			{
				return View(createProduct);
			}
			bool result = await _context.Products.AnyAsync(p => p.Name.Trim().ToLower() == createProduct.Name.ToLower().Trim());
			if (result)
			{
				ModelState.AddModelError("Name", "Name is exists");
				return View(createProduct);
			}
			if (!createProduct.Photo.ValidateType("image/"))
			{
				ModelState.AddModelError("Photo", "Photo type is unavailable");
				return View(createProduct);
			}
			if (!createProduct.Photo.ValidateSize(2*1024))
			{
				ModelState.AddModelError("Photo", "Photo Size max 2mb");
				return View(createProduct);
			}
			string filename = await createProduct.Photo.CreateFilesAsync(_env.WebRootPath, "assets", "max-image");
			Product product = new Product
			{
				Image = filename,
				Name = createProduct.Name,
				Price = createProduct.Price,
			};
			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Update(int id)
		{
			if (id<=0)
			{
				return BadRequest();
			}
			Product product= await _context.Products.FirstOrDefaultAsync(p=> p.Id == id);
			if (product==null)
			{
				return NotFound();
			}

			UpdateProductVM updateProductVM = new UpdateProductVM
			{
				Image = product.Image,
				Name = product.Name,
				Price = product.Price,

			};
			return View(updateProductVM);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id,UpdateProductVM updateProductVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			bool result = await _context.Products.AnyAsync(p => p.Name.Trim().ToLower() == updateProductVM.Name.ToLower().Trim()&& p.Id!=id);
			if (result)
			{
				ModelState.AddModelError("Name", "Name is exists");
				return View(updateProductVM);
			}

			if (updateProductVM.Photo is not null)
			{
				if (!updateProductVM.Photo.ValidateType("image/"))
				{
					ModelState.AddModelError("Photo", "Photo type is unavailable");
					return View();
				}
				if (!updateProductVM.Photo.ValidateSize(2 * 1024))
				{
					ModelState.AddModelError("Photo", "Photo Size max 2mb");
					return View(updateProductVM);
				}
				string newImage = await updateProductVM.Photo.CreateFilesAsync(_env.WebRootPath, "assets", "max-image");
				product.Image.DeleteFile(_env.WebRootPath, "assets", "max-image");
				product.Image= newImage;
			}

			product.Name= updateProductVM.Name;
			product.Price= updateProductVM.Price;
			 
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (id<=0)
			{
				return BadRequest();
			}
			Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			product.Image.DeleteFile(_env.WebRootPath, "assets","max-image");
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		
	}
}
