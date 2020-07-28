using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Entities.Custom
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ImageAddress { get; set; }

        public string Base64Image { get; set; }
        public string Base64Thumbnil{ get; set; }

    }
}