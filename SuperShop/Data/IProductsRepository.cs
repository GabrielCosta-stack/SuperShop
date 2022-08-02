using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SuperShop.Data
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboProducts();
    }
}
