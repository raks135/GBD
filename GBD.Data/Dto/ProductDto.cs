using System.Collections.Generic;

namespace GBD.Data.Dto
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal Discount { get; set; }


		public string Rating { get; set; }
		public HrefLink Href { get; set; }
		public  virtual ProductDetailDto ProductDetail { get; set; }
	}
	public class HrefLink
	{
		public string Link { get; set; }
	}
}
