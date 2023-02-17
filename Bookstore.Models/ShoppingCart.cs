using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "You can only buy 100 of these")]
        public int Count { get; set; }
    }
}
