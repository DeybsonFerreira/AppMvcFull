using AppMvcFull.App.Extensions;
using AppMvcFull.App.Models;
using AppMvcFull.App.Utils;
using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.App.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductRepository productRepository,
            ISupplierRepository supplierRepository,
            IMapper mapper,
            IWebHostEnvironment env,
            INotification notification) : base(notification, env, logger)
        {
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        [Route("produtos")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Read)]
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _productRepository.GetAllProductSupplierAsync();
            IEnumerable<ProductViewModel> modelView = _mapper.Map<IEnumerable<ProductViewModel>>(products);
            return View(modelView);
        }

        [Route("produtos/detalhes/{id:guid}")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Read)]
        public async Task<IActionResult> Details(Guid id)
        {
            Product product = await _productRepository.GetProductSupplierAsync(id);

            if (product == null)
                return NotFound();

            ProductViewModel modelView = _mapper.Map<ProductViewModel>(product);
            return View(modelView);
        }

        [Route("produtos/excluir/{id:guid}")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null)
                return NotFound();

            ProductViewModel modelView = _mapper.Map<ProductViewModel>(product);
            return View(modelView);
        }

        [HttpPost, ActionName("Delete")]
        [Route("produtos/excluir/{id:guid}")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Delete)]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product != null)
            {
                await _productRepository.RemoveAsync(product.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("produtos/novo")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Create)]
        public async Task<IActionResult> Create()
        {
            var modelView = new ProductViewModel
            {
                Suppliers = await GetSuppliersModelView()
            };

            return View(modelView);
        }

        [HttpPost]
        [Route("produtos/novo")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Create)]
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

        [Route("produtos/editar/{id:guid}")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Update)]
        public async Task<IActionResult> Edit(Guid id)
        {
            ProductViewModel modelView = _mapper.Map<ProductViewModel>(await _productRepository.GetProductSupplierAsync(id));
            return View(modelView);
        }

        [HttpPost]
        [Route("produtos/editar/{id:guid}")]
        [ClaimsAuthorize(ConstantClaimName.ProductsClaimName, ConstantClaimValue.Update)]
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

            return View(modelView);
        }

        private async Task<IEnumerable<SupplierViewModel>> GetSuppliersModelView()
        {
            return _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAllAsync());
        }
    }
}