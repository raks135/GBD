using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GBD
{
	public class HttpGlobalExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<HttpGlobalExceptionFilter> logger;

		public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
		{
			this.logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			logger.LogError(new EventId(context.Exception.HResult),
					context.Exception,
					context.Exception.Message);

			context.ExceptionHandled = true;
		}

		private class JsonErrorResponse
		{
			public string[] Messages { get; set; }

			public object DeveloperMeesage { get; set; }
		}


	}
}
