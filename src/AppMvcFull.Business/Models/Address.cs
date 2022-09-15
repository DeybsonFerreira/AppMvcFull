using System;
using System.ComponentModel.DataAnnotations;

namespace AppMvcFull.Business.Models
{
    public class Address : Entity
    {
        public string StreetName { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string ZipCode { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        /*Propriedade de navegação*/
        public Guid SupplierId { get; set; }

        /*EF Relations*/
        public Supplier Supplier { get; set; }
    }
}
