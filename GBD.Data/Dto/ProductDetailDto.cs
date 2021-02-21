using GBD.Data.Models;
using System.Collections.Generic;

namespace GBD.Data.Dto
{
	public class ProductDetailDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
		public string Price { get; set; }
		public string Stock { get; set; }
		public decimal? Discount { get; set; }
		public string Rating { get; set; }
		public decimal? TotalPrice { get; set; }
		public HrefReview Href { get; set; }
		public virtual Product Product { get; set; }

		public virtual List<ReviewDto> Reviews { get; set; }
	}
	public class HrefReview
	{
		public string Reviews { get; set; }
	}
}
