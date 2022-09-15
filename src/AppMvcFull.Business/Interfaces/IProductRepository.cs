using AppMvcFull.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductSupplierAsync(Guid id);
        Task<List<Product>> GetAllProductSupplierAsync();
        Task<IEnumerable<Product>> GetBySupplierIdAsync(Guid supplierId);
    }
}
