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
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository, IAddressRepository addressRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        [Route("fornecedor")]
        public async Task<IActionResult> Index()
        {
            List<Supplier> suppliers = await _supplierRepository.GetAllAsync();
            IEnumerable<SupplierViewModel> modelView = _mapper.Map<IEnumerable<SupplierViewModel>>(suppliers);
            return View(modelView);
        }

        [Route("fornecedor/detalhes/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            Supplier supplier = await _supplierRepository.GetSupplierWithAddressAndProductsAsync(id);

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

        public async Task<IActionResult> GetAddressPopup(Guid id)
        {
            var supplier = await _supplierRepository.GetSupplierWithAddressAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);

            return PartialView("_AddressUpdateModal", new SupplierViewModel() { Address = modelView.Address });
        }

        public async Task<IActionResult> GetAddressDetail(Guid id)
        {
            var supplier = await _supplierRepository.GetSupplierWithAddressAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);

            return PartialView("_AddressDetail", modelView);
        }

        [HttpPost]
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
            Supplier supplier = await _supplierRepository.GetSupplierWithAddressAsync(id);

            if (supplier == null)
                return NotFound();

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);
            return View(modelView);
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> UpdateAddress(SupplierViewModel supplierModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("DocumentNumber");

            if (!ModelState.IsValid)
                return PartialView("_AddressUpdateModal", supplierModel);

            await _addressRepository.UpdateAsync(_mapper.Map<Address>(supplierModel.Address));

            var url = Url.Action("GetAddressDetail", "Suppliers", new { id = supplierModel.Address.SupplierId });
            return Json(new { success = true, url });
        }
    }

}
