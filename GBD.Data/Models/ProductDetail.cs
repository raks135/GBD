using System;
using System.Collections.Generic;

#nullable disable

namespace GBD.Data.Models
{
    public partial class ProductDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Price { get; set; }
        public int? Stock { get; set; }
        public decimal? Discount { get; set; }
        public int? Rating { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime InsertDate { get; set; }
        public string InsertedBy { get; set; }

        public virtual Product Product { get; set; }
    }
}
