using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        public IOrderRepository _orderRepository { get; }
        public IProductsRepository _productsRepository { get; }

        public OrdersController(IOrderRepository orderRepository, IProductsRepository productsRepository)
        {
            _orderRepository = orderRepository;
            _productsRepository = productsRepository;
        }

       

        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailTempsAsync(this.User.Identity.Name);

            return View(model);
        }

        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel {
                Quantity = 1,
                Products = _productsRepository.GetComboProducts()
            };

            return View(model); 
        }
    }
}
