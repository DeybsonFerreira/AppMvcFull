using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.Data.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppMvcFullDbContext db) : base(db)
        {
        }
        public async Task<List<Supplier>> GetAllSupplierIncludesAsync()
        {
            return await _context.Suppliers.AsNoTracking().Include(f => f.Products).Include(f => f.Address).ToListAsync();
        }
    }
}
