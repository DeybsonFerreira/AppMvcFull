using AppMvcFull.Business.Models;
using System;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetBySupplierId(Guid supplierId);
    }
}
