using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevIO.App.ViewModels
{
    public class SupplierViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(100, ErrorMessage = "{0} field must be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(14, ErrorMessage = "{0} field must be between {2} and {1} characters", MinimumLength = 11)]
        public string Document { get; set; }

        public int SupplierType { get; set; }

        public bool Active { get; set; }

        /* EF - Relations */

        public AddressViewModel Address { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}