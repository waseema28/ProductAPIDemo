using DataAccess.DatabaseObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniessLogic.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetProductsAsync(int page, int pageSize);
        Task<Products> GetProductByIdAsync(int id);
        Task<Products> CreateProductAsync(Products product);
        Task<Products> UpdateProductAsync(Products product);
        Task<bool> DeleteProductAsync(int id);
    }
}
