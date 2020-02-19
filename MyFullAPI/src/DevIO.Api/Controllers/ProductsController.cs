using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/products")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
                                  IProductService productService,
                                  IMapper mapper,
                                  INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsAndSuppliers());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> GetById(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            return productViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> Add(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + productViewModel.Image;

            if (!UploadFile(productViewModel.UploadImage, imageName))
            {
                return CustomResponse(productViewModel);
            }

            productViewModel.Image = imageName;

            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifyError("The id given is not the same as the one entered in the query");
                return CustomResponse();
            }

            var productUpdate = await GetProduct(id);
            productViewModel.Image = productUpdate.Image;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (productViewModel.UploadImage != null)
            {
                var imageName = Guid.NewGuid() + "_" + productViewModel.Image;

                if (!UploadFile(productViewModel.UploadImage, imageName))
                {
                    return CustomResponse(ModelState);
                }

                productUpdate.Image = imageName;
            }

            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Price = productViewModel.Price;
            productViewModel.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(productUpdate));

            return CustomResponse(productViewModel);
        }

        [HttpPost("add-alternative")]
        public async Task<ActionResult<ProductViewModel>> AddAlternative(ProductImageViewModel productImageViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imagePrefix = Guid.NewGuid() + "_";

            if (!await UploadAlternativeAsync(productImageViewModel.UploadImage, imagePrefix))
            {
                return CustomResponse(productImageViewModel);
            }

            productImageViewModel.Image = imagePrefix + productImageViewModel.UploadImage.FileName;

            await _productService.Add(_mapper.Map<Product>(productImageViewModel));

            return CustomResponse(productImageViewModel);
        }

        //[DisableRequestSizeLimit]
        [RequestSizeLimit(15000)]
        [HttpPost("image")]
        public ActionResult AddImage(IFormFile file)
        {
            return Ok(file);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductViewModel>> Delete(Guid id)
        {
            var product = await GetProduct(id);

            if (product == null) return NotFound();

            await _productService.Remove(id);

            return CustomResponse(product);
        }

        private bool UploadFile(string file, string imageName)
        {
            if (string.IsNullOrEmpty(file))
            {
                NotifyError("Please provide an image for this product!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(file);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imageName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("There is already a file with that name!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }

        private async Task<bool> UploadAlternativeAsync(IFormFile file, string imagePrefix)
        {
            if (file == null || file.Length == 0)
            {
                NotifyError("Please provide an image for this product!");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/app/demo-webapi/src/assets", imagePrefix + file.FileName);

            if (System.IO.File.Exists(path))
            {
                NotifyError("There is already a file with that name!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            return _mapper.Map<ProductViewModel>(await _productRepository.GetProductAndSupplier(id));
        }
    }
}
