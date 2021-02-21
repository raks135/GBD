using AutoMapper;
using GBD.Data;
using GBD.Data.Dto;
using GBD.Data.Models;
using GBD.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GBD.Service.Service
{
	public class ProductService : IProductService
	{
		private readonly GBDContext _dbContext;
		private readonly IMapper _mapper;

		public ProductService(GBDContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}
		public void GetProduct()
		{
			Uri baseUrl = new Uri(ApiUri.API_URI);
			IRestClient client = new RestClient(baseUrl);
			IRestRequest request = new RestRequest(Method.GET);

			request.AddHeader("api-key", "$2y$10$tAajJXlhdqDfGi8CppFN3.KWnofLUVE03gknOyEDv9OBAcypda9MO");
			request.AddHeader("Content-Type", "application/json");

			var responseBody = client.Execute(request).Content;

			var parsedObject = JObject.Parse(responseBody);
			var dataFromApi = JsonConvert.DeserializeObject<List<ProductDto>>(parsedObject.GetValue("data").ToString()).Take(1);

			//ProductDetail

			foreach (var item in dataFromApi)
			{
				client.BaseUrl = new Uri(item.Href.Link);
				IRestRequest productDetailRequest = new RestRequest(Method.GET);
				productDetailRequest.AddHeader("api-key", "$2y$10$tAajJXlhdqDfGi8CppFN3.KWnofLUVE03gknOyEDv9OBAcypda9MO");
				productDetailRequest.AddHeader("Content-Type", "application/json");
				var productDetailResponseBody = JObject.Parse(client.Execute(productDetailRequest).Content);

				var productDetailFromApi = JsonConvert.DeserializeObject<ProductDetailDto>(productDetailResponseBody.GetValue("data").ToString());
				item.ProductDetail = productDetailFromApi;

			}

			//Review
			foreach (var review in dataFromApi)
			{
				client.BaseUrl = new Uri(review.ProductDetail.Href.Reviews);
				IRestRequest reviewRequest = new RestRequest(Method.GET);
				reviewRequest.AddHeader("api-key", "$2y$10$tAajJXlhdqDfGi8CppFN3.KWnofLUVE03gknOyEDv9OBAcypda9MO");
				reviewRequest.AddHeader("Content-Type", "application/json");
				var reviewResponseBody = JObject.Parse(client.Execute(reviewRequest).Content);

				var reviewFromApi = JsonConvert.DeserializeObject<List<ReviewDto>>(reviewResponseBody.GetValue("data").ToString());
				review.ProductDetail.Reviews = reviewFromApi;

			}
			var dataToSave = JsonConvert.SerializeObject(dataFromApi);

			using (_dbContext)
			{
				var sql = "EXEC dbo.Sp_ProductDataIns @jsonData = {0}";
				_dbContext.Database.ExecuteSqlRaw(sql, dataFromApi);
			}
		}


	}
}
