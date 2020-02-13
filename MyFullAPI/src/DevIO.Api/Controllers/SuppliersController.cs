using AutoMapper;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/suppliers")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAll());
        }
    }
}
