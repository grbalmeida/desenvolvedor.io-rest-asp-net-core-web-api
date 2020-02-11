﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DevIO.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        public Guid SupplierId { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(200, ErrorMessage = "{0} field must be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        [StringLength(1000, ErrorMessage = "{0} field must be between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }

        public string UploadImage { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "The {0} field is required")]
        public decimal Price { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

        public bool Active { get; set; }

        [ScaffoldColumn(false)]
        public string SupplierName { get; set; }
    }
}