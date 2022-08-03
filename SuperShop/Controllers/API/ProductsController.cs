using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;

namespace SuperShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    // Bloqueia a API, acesso apenas com token
    // Necessita configuração no ficheiro startup.cs
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {
        public IProductsRepository _productRepository;

        public ProductsController(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAllWithUsers());
        }

       
    }
}
