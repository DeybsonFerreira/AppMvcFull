using AppMvcFull.Business.Models;
using System;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Interfaces
{
    public interface ISupplierServices : IDisposable
    {
        Task CreateAsync(Supplier model);
        Task UpdateAsync(Supplier model);
        Task UpdateAddressAsync(Address model);
        Task DeleteAsync(Guid supplerId);
    }
}