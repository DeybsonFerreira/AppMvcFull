using AppMvcFull.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<List<Supplier>> GetAll();
        Task<List<Supplier>> GetAllWithAddressAndProductAsync();
        Task<Supplier> GetSupplierWithAddressAsync(Guid supplierId);
        Task<Supplier> GetSupplierWithAddressAndProductsAsync(Guid supplierId);
    }
}
