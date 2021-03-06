﻿using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.Extensions;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/suppliers")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SuppliersController(ISupplierRepository supplierRepository,
                                   ISupplierService supplierService,
                                   IAddressRepository addressRepository,
                                   IMapper mapper,
                                   INotifier notifier,
                                   IUser user,
                                   ILogger<SuppliersController> logger) : base(notifier, user)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _addressRepository = addressRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<SupplierViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> GetById(Guid id)
        {
            var supplier = await GetAddressAndProductsFromSupplier(id);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier not found");
                return NotFound();
            }

            return supplier;
        }

        [ClaimsAuthorize("Supplier", "Add")]
        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Add(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.Add(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Update")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Update(Guid id, SupplierViewModel supplierViewModel)
        {
            if (id != supplierViewModel.Id)
            {
                _logger.LogWarning("The id given is not the same as the one entered in the query");
                NotifyError("The id given is not the same as the one entered in the query");
                return CustomResponse(supplierViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.Update(_mapper.Map<Supplier>(supplierViewModel));

            return CustomResponse(supplierViewModel);
        }

        [ClaimsAuthorize("Supplier", "Delete")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            var supplierViewModel = await GetSupplierAddress(id);

            if (supplierViewModel == null)
            {
                _logger.LogWarning("Supplier not found");
                return NotFound();
            }

            await _supplierService.Remove(id);

            return CustomResponse(supplierViewModel);
        }

        [HttpGet("get-address/{id:guid}")]
        public async Task<AddressViewModel> GetAddressById(Guid id)
        {
            return _mapper.Map<AddressViewModel>(await _addressRepository.GetById(id));
        }

        [ClaimsAuthorize("Supplier", "Update")]
        [HttpPut("update-address/{id:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _supplierService.UpdateAddress(_mapper.Map<Address>(addressViewModel));

            return CustomResponse(addressViewModel);
        }

        private async Task<SupplierViewModel> GetAddressAndProductsFromSupplier(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetAddressAndProductsFromSupplier(id));
        }

        private async Task<SupplierViewModel> GetSupplierAddress(Guid id)
        {
            return _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierAddress(id));
        }
    }
}
