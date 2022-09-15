using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Data.Context;

namespace AppMvcFull.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(AppMvcFullDbContext db) : base(db)
        {
        }
    }
}
