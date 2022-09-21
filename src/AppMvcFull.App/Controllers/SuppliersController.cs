using AppMvcFull.App.Extensions;
using AppMvcFull.App.ViewModels;
using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.App.Controllers
{
    public class SuppliersController : BaseController
    {
        private readonly ISupplierServices _supplierServices;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public SuppliersController(
            ISupplierServices supplierServices,
            ISupplierRepository supplierRepository,
            IAddressRepository addressRepository,
            IMapper mapper,
            INotification notification) : base(notification)
        {
            _supplierServices = supplierServices;
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

        [Route("fornecedor/novo")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("fornecedor/excluir")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            if (supplier == null)
                return NotFound();

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);
            return View(modelView);
        }

        [Route("fornecedor/popup/{id:guid}")]
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

        [Route("fornecedor/endereco/detalhes/{id:guid}")]
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

        [Route("fornecedor/editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Supplier supplier = await _supplierRepository.GetSupplierWithAddressAsync(id);

            if (supplier == null)
                return NotFound();

            SupplierViewModel modelView = _mapper.Map<SupplierViewModel>(supplier);
            return View(modelView);
        }

        [Route("fornecedor/novo")]
        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel modelView)
        {
            if (ModelState.IsValid)
            {
                modelView.Id = Guid.NewGuid();
                Supplier model = _mapper.Map<Supplier>(modelView);
                await _supplierServices.CreateAsync(model);

                if (!CheckValidation())
                    return View(modelView);

                TempData["Sucesso"] = "Fornecedor Criado com Sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(modelView);
        }

        [HttpPost]
        [Route("fornecedor/editar/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, SupplierViewModel modelView)
        {
            if (id != modelView.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    Supplier model = _mapper.Map<Supplier>(modelView);
                    await _supplierServices.UpdateAsync(model);

                    if (!CheckValidation())
                        return View(modelView);

                    TempData["Sucesso"] = "Fornecedor Atualizado com Sucesso!";
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
        public async Task<IActionResult> UpdateAddress(SupplierViewModel modelView)
        {
            ModelState.Remove("Name");
            ModelState.Remove("DocumentNumber");

            if (!ModelState.IsValid)
                return PartialView("_AddressUpdateModal", modelView);

            await _supplierServices.UpdateAddressAsync(_mapper.Map<Address>(modelView.Address));

            if (!CheckValidation())
                return View(modelView);

            TempData["Sucesso"] = "Endereço Atualizado com Sucesso!";
            var url = Url.Action("GetAddressDetail", "Suppliers", new { id = modelView.Address.SupplierId });
            return Json(new { success = true, url });
        }

        [HttpPost, ActionName("Delete")]
        [Route("fornecedor/excluir")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var supplier = await _supplierRepository.GetAsync(id);
            if (supplier != null)
            {
                await _supplierServices.DeleteAsync(supplier.Id);
                if (!CheckValidation())
                    return View(_mapper.Map<SupplierViewModel>(supplier));

                TempData["Sucesso"] = "Fornecedor Excluído com Sucesso!";
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
