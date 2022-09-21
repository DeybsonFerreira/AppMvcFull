using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AppMvcFull.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppMvcFullDbContext db) : base(db)
        {
        }

        public async Task<Address> GetBySupplierId(Guid supplierId)
        {
            return await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(f => f.SupplierId == supplierId);
        }

    }
}
