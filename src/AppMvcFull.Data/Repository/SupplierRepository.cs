using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.Data.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppMvcFullDbContext db) : base(db)
        {
        }

        public async Task<List<Supplier>> GetAll()
        {
            return await _context.Suppliers.AsNoTracking()
                                           .ToListAsync();
        }

        public async Task<List<Supplier>> GetAllWithAddressAndProductAsync()
        {
            return await _context.Suppliers.AsNoTracking()
                                           .Include(f => f.Products)
                                           .Include(f => f.Address)
                                           .ToListAsync();
        }

        public async Task<Supplier> GetSupplierWithAddressAsync(Guid supplierId)
        {
            return await _context.Suppliers.AsNoTracking()
                                           .Include(f => f.Address)
                                            .FirstOrDefaultAsync(f => f.Id == supplierId);
        }
        public async Task<Supplier> GetSupplierWithAddressAndProductsAsync(Guid supplierId)
        {
            return await _context.Suppliers.AsNoTracking()
                                           .Include(f => f.Address)
                                           .Include(f => f.Products)
                                           .FirstOrDefaultAsync(f => f.Id == supplierId);
        }
    }
}
