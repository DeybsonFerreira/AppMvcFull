using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppMvcFullDbContext db) : base(db)
        {
        }

        public async Task<List<Product>> GetAllProductSupplierAsync()
        {
            return await _context.Products.AsNoTracking().Include(f => f.Supplier).ToListAsync();
        }

        public async Task<Product> GetProductSupplierAsync(Guid id)
        {
            return await _context.Products.AsNoTracking().Include(f => f.Supplier).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<Product>> GetBySupplierIdAsync(Guid supplierId)
        {
            return await FindAsync(c => c.SupplierId == supplierId);
        }
    }
}
