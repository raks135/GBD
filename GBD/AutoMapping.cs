using AutoMapper;
using GBD.Data.Dto;
using GBD.Data.Models;

namespace GBD
{
	public class AutoMapping : Profile
	{
		public AutoMapping()
		{
			CreateMap<ProductDto, Product>().
				ForMember(dest => dest.Rating,
				opt => opt.MapFrom(src => src.Rating.Contains("No") ? 0 : int.Parse(src.Rating)));

			CreateMap<ProductDetailDto, ProductDetail>()
					.ForMember(dest => dest.Stock,
					opt => opt.MapFrom(src => src.Stock.Contains("No") ? 0 : int.Parse(src.Stock)))
						.ForMember(dest => dest.Rating,
					opt => opt.MapFrom(src => src.Rating.Contains("No") ? 0 : int.Parse(src.Rating)));
			CreateMap<ReviewDto, ReviewDto>();


		}

	}
}
