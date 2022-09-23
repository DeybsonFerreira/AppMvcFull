using AppMvcFull.App.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppMvcFull.App.Models
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Description { get; set; }

        [DisplayName("Imagem")]
        public IFormFile ImageUpload { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Valor")]
        [MoneyValidation]
        public decimal Value { get; set; }

        [DisplayName("Ativo ?")]
        public bool Active { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Data de criação")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid SupplierId { get; set; }

        public SupplierViewModel Supplier { get; set; }
        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}
