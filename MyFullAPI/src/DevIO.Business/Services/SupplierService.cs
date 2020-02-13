using System;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierService(
            ISupplierRepository supplierRepository,
            IAddressRepository addressRepository,
            INotifier notifier) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Supplier supplier)
        {
            if (!PerformValidation(new SupplierValidation(), supplier)
                || !PerformValidation(new AddressValidation(), supplier.Address)) return false;

            if (_supplierRepository.Search(s => s.Document == supplier.Document).Result.Any())
            {
                Notify("There is already a supplier with this document entered.");
                return false;
            }

            await _supplierRepository.Add(supplier);

            return true;
        }

        public async Task<bool> Update(Supplier supplier)
        {
            if (!PerformValidation(new SupplierValidation(), supplier)) return false;
        
            if (_supplierRepository.Search(s => s.Document == supplier.Document && s.Id != supplier.Id).Result.Any())
            {
                Notify("There is already a supplier with this document entered.");
                return false;
            }

            await _supplierRepository.Update(supplier);

            return true;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!PerformValidation(new AddressValidation(), address)) return;

            await _addressRepository.Update(address);
        }

        public async Task<bool> Remove(Guid id)
        {
            if (_supplierRepository.GetAddressAndProductsFromSupplier(id).Result.Products.Any())
            {
                Notify("The supplier has registered products!");
                return false;
            }

            await _supplierRepository.Remove(id);

            return true;
        }

        public void Dispose()
        {
            _supplierRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}
