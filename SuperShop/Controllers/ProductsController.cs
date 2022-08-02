using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    //[Authorize]
    public class ProductsController : Controller
    {
        private IProductsRepository _productRepository;
        private readonly IUserHelper _userHelper;
        //private readonly IImageHelper _imageHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public ProductsController(
            IProductsRepository  productRepository,
            IUserHelper userHelper,
            //IImageHelper imageHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper
            )
        {
           _productRepository = productRepository;
           _userHelper = userHelper;
            //_imageHelper = imageHelper;
           _blobHelper = blobHelper;
           _converterHelper = converterHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(product);
        }

        // GET: Products/Create
       
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                //var path = string.Empty;

                if(model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    //path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");

                    // nome da pasta´é a que está no AZURE, contentores
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");

                }

                //var product = _converterHelper.ToProduct(model, path, true);
                var product = _converterHelper.ToProduct(model, imageId, true);

                // Atribui o user que está logado ao produto
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _productRepository.CreateAsync(product);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ToProductViewModel(product);


            return View(model);
        }

      

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    //var path = model.ImageUrl;
                    Guid imageId = model.ImageId; // Está na página Edit como inputHiden,
                                                  // mudar na view no projeto Leasing

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        //path = await _imageHelper.UploadImageAsync(model.ImageFile, "products");
                        // nome da pasta´é a que está no AZURE, contentores
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                    }

                    //var product = _converterHelper.ToProduct(model, path, false);
                    var product = _converterHelper.ToProduct(model, imageId, false);


                    //  MODIFICA PARA O USER QUE ESTIVER LOGADO
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _productRepository.UpdateAsync(product);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("ProductNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        [Authorize]
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ToProductViewModel(product);

            return View(model);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Apenas Admin pode criar produtos
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            try
            {
                
                await _productRepository.DeleteAsync(product);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if(ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Name} provavelmente está a ser usado!";
                    ViewBag.ErrorMessage = $"{product.Name} não pode ser apagado visto haver encomendas que o usam. <br/>" +
                        $"Experimente primeiro apagar todas as encomendas que estão a usar," +
                        $"e torne novamente a apagá-lo";
                }
                

                return View("Error");
            }
           

           

           
        }

        public IActionResult ProductNotFound()
        {
            return View();
        }

      
    }
}
