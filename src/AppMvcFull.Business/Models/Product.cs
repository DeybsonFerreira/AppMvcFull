using System;
using System.ComponentModel.DataAnnotations;

namespace AppMvcFull.Business.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public decimal Value { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }

        /*Propriedade de navegação*/
        public Guid SupplierId { get; set; }

        /*EF Relations*/
        public Supplier Supplier { get; set; }

    }
}
