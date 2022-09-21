using AppMvcFull.Business.Interfaces;
using AppMvcFull.Business.Models;
using AppMvcFull.Business.Validations;
using System;
using System.Threading.Tasks;

namespace AppMvcFull.Business.Services
{
    /// <summary>
    /// Classes para regra de negócios, não precisa ter retorno
    /// </summary>
    public class ProductServices : BaseServices, IProductServices
    {
        private readonly IProductRepository _productRepository;

        public ProductServices(IProductRepository produtoRepository, INotification notification) : base(notification)
        {
            _productRepository = produtoRepository;
        }

        public async Task CreateAsync(Product model)
        {
            if (!ExecuteValidation(new ProductValidation(), model))
                return;

            await _productRepository.AddAsync(model);
        }

        public async Task UpdateAsync(Product produto)
        {
            if (!ExecuteValidation(new ProductValidation(), produto))
                return;

            await _productRepository.UpdateAsync(produto);
        }

        public async Task DeleteAsync(Guid productId)
        {
            await _productRepository.RemoveAsync(productId);
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }


    }
}
