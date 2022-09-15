using AppMvcFull.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        Task<List<Supplier>> GetAllSupplierIncludesAsync();

    }
}
