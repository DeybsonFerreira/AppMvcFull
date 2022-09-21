using AppMvcFull.Business.Models;
using System;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Interfaces
{
    public interface IProductServices : IDisposable
    {
        Task CreateAsync(Product model);
        Task UpdateAsync(Product model);
        Task DeleteAsync(Guid productId);
    }
}