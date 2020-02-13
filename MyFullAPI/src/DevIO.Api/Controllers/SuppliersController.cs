using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/suppliers")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   ISupplierService supplierService,
                                   IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> GetById(Guid id)
        {
            var supplier = await GetAddressAndProductsFromSupplier(id);

            if (supplier == null) return NotFound();

            return supplier;
        }

        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Add(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var supplier = _mapper.Map<Supplier>(supplierViewModel);

            var result = await _supplierService.Add(supplier);

            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Update(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            var result = await _supplierService.Update(supplier);

            if (!result) return BadRequest();

            return Ok(supplier);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplier = await GetSupplierAddress(id);

            if (supplier == null) return NotFound();

            var result = await _supplierService.Remove(id);

            if (!result) return BadRequest();

            return Ok(supplier);
        }

        public async Task<SupplierViewModel> GetAddressAndProductsFromSupplier(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetAddressAndProductsFromSupplier(id));
        }

        public async Task<SupplierViewModel> GetSupplierAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddress(id));
        }
    }
}
