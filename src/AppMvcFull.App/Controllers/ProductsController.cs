using AppMvcFull.App.ViewModels;
using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AppMvcFull.App.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ProductsController> _logger;


        public ProductsController(
            ILogger<ProductsController> logger,
            IProductRepository productRepository,
            ISupplierRepository supplierRepository,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productRepository.GetAllProductSupplierAsync();
            IEnumerable<ProductViewModel> modelView = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            return View(modelView);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Product product = await _productRepository.GetProductSupplierAsync(id);

            if (product == null)
                return NotFound();

            ProductViewModel modelView = _mapper.Map<ProductViewModel>(product);
            return View(modelView);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();

            ProductViewModel modelView = _mapper.Map<ProductViewModel>(product);
            return View(modelView);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product != null)
            {
                await _productRepository.RemoveAsync(product.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            var modelView = new ProductViewModel
            {
                Suppliers = await GetSuppliersModelView()
            };

            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel modelView)
        {
            if (ModelState.IsValid)
            {
                modelView.Id = Guid.NewGuid();
                modelView.Image = "empty.png";
                modelView.CreatedDate = DateTime.Now;

                bool imgSaved = await SaveProductImage(modelView);
                if (!imgSaved)
                    return View(modelView);

                Product model = _mapper.Map<Product>(modelView);

                await _productRepository.AddAsync(model);

                return RedirectToAction(nameof(Index));
            }
            return View(modelView);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            ProductViewModel modelView = _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplierAsync(id));
            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel modelView)
        {
            if (id != modelView.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //reload data before to change
                    var productDb = await _productRepository.GetProductSupplierAsync(id);
                    modelView.Image = productDb.Image;
                    modelView.SupplierId = productDb.SupplierId;
                    modelView.Supplier = _mapper.Map<SupplierViewModel>(productDb.Supplier);

                    bool imgSaved = await SaveProductImage(modelView);
                    if (!imgSaved)
                        return View(modelView);

                    Product model = _mapper.Map<Product>(modelView);
                    await _productRepository.UpdateAsync(model);
                    await _productRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var modelExist = await _productRepository.GetAsync(id);
                    if (modelExist == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            //ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "Id", "Id", product.SupplierId);
            return View(modelView);
        }

        private async Task<IEnumerable<SupplierViewModel>> GetSuppliersModelView()
        {
            return _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync());
        }

        private async Task<bool> SaveProductImage(ProductViewModel modelView)
        {
            if (modelView.ImageUpload != null)
            {
                string imgPrefix = $"{modelView.Id}.jpg";
                var createFolder = await UploadImage(modelView.ImageUpload, imgPrefix, "Product");
                if (createFolder)
                    modelView.Image = imgPrefix;

                return createFolder;
            }
            return true;
        }
        private async Task<bool> UploadImage(IFormFile file, string imgPrefix, string folderType)
        {
            if (file.Length <= 0) return false;
            decimal imgCurrent = decimal.Round(file.Length / 1024M / 1024M, 2);
            decimal maxMegaByte = 3M;
            if (imgCurrent > maxMegaByte)
            {
                ModelState.AddModelError("", $"Imagem contém {imgCurrent}MB não pode ser maior que {maxMegaByte}MB");
                return false;
            }

            Uri resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            string folderPath = Path.Combine(_env.ContentRootPath, "wwwroot/Files", folderType);
            try
            {

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                folderPath += $"/{imgPrefix}";

                if (System.IO.File.Exists(folderPath))
                    System.IO.File.Delete(folderPath);

                await using FileStream fs = new(folderPath, FileMode.CreateNew);
                await file.CopyToAsync(fs);
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("", $"Erro ao criar imagem <Log> ${ex.Message}");
                _logger.LogError($"Error on create file on {resourcePath} | {ex.Message}");
                return false;
            }

            return true;
        }

    }
}