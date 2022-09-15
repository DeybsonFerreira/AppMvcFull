using AppMvcFull.App.ViewModels;
using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.App.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<Supplier> suppliers = await _supplierRepository.GetAllSupplierIncludesAsync();
            IEnumerable<SupplierViewModel> modelView = _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
            return View(modelView);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            Supplier supplier = await _supplierRepository.GetAsync(id);

            if (supplier == null)
                return NotFound();

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);
            return View(modelView);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            if (supplier == null)
                return NotFound();

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);
            return View(modelView);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            if (supplier != null)
            {
                await _supplierRepository.RemoveAsync(supplier.Id);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel modelView)
        {
            if (ModelState.IsValid)
            {
                modelView.Id = Guid.NewGuid();
                Supplier model = _mapper.Map<Supplier>(modelView);
                await _supplierRepository.AddAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View(modelView);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            Supplier supplier = await _supplierRepository.GetAsync(id);

            if (supplier == null)
                return NotFound();

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);
            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel modelView)
        {
            if (id != modelView.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Supplier model = _mapper.Map<Supplier>(modelView);
                    await _supplierRepository.UpdateAsync(model);
                    await _supplierRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var modelExist = await _supplierRepository.GetAsync(id);
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
    }

}
