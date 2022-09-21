using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Business.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Services
{
    /// <summary>
    /// Classes para regra de negócios, não precisa ter retorno
    /// </summary>
    public class SupplierServices : BaseServices, ISupplierServices
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierServices(ISupplierRepository supplierRepository,
                                 IAddressRepository addressRepository,
                                 INotification notification) : base(notification)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task CreateAsync(Supplier model)
        {
            if (!ExecuteValidation(new SupplierValidation(), model))
                return;

            if (!ExecuteValidation(new SupplierValidation(), model) || !ExecuteValidation(new AddressValidation(), model.Address))
                return;

            if (_supplierRepository.FindAsync(f => f.DocumentNumber == model.DocumentNumber).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento infomado.");
                return;
            }

            await _supplierRepository.AddAsync(model);
        }

        public async Task UpdateAsync(Supplier model)
        {
            if (!ExecuteValidation(new SupplierValidation(), model)) return;

            if (_supplierRepository.FindAsync(f => f.DocumentNumber == model.DocumentNumber && f.Id != model.Id).Result.Any())
            {
                Notify("Já existe um fornecedor com este documento infomado.");
                return;
            }

            await _supplierRepository.UpdateAsync(model);
        }

        public async Task UpdateAddressAsync(Address model)
        {
            if (!ExecuteValidation(new AddressValidation(), model))
                return;

            await _addressRepository.UpdateAsync(model);
        }

        public async Task DeleteAsync(Guid supplerId)
        {
            var supplierdb = _supplierRepository.GetSupplierWithAddressAndProductsAsync(supplerId).Result;
            if (supplierdb.Products.Any())
            {
                Notify("O fornecedor possui produtos cadastrados!");
                return;
            }

            if (supplierdb.Address != null)
                await _addressRepository.RemoveAsync(supplierdb.Address.Id);

            await _supplierRepository.RemoveAsync(supplerId);
        }

        public void Dispose()
        {
            _supplierRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}
