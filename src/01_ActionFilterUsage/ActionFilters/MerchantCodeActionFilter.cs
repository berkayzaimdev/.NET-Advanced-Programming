using _01_ActionFilterUsage.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _01_ActionFilterUsage.ActionFilters;

public class MerchantCodeActionFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var key = "merchantCode"; // controller'daki {merchantCode}'a karşılık gelir. camelcase olmak zorunda
		// get data

		var merchantCode = context.RouteData.Values[key]; // bu isimde bir key oluşturdu. bu kullanım sayesinde kodu query string yerine path olarak veriyoruz

		// set data

		var baseRequest = context
			.ActionArguments.FirstOrDefault(a => a.Value != null && a.Value.GetType().Equals(typeof(MerchantBaseRequest)));

		if (baseRequest.Value is not null)
		{
			var req = baseRequest.Value as MerchantBaseRequest;

			req.MerchantCode = merchantCode.ToString(); // path olarak verdiysek request'i güncelle
		}

        else
        {
			if (!context.ActionArguments.ContainsKey(key))
			{
				context.ActionArguments.Add(key, merchantCode);
			}

			else 
			  context.ActionArguments[key] = merchantCode;
        }

        await next();
	}
}

public class MerchantCodeActionFilterAttribute : ActionFilterAttribute // filter'ı sadece metoda ya da controller'a has kılmak adına bu kullanımı yapabilmekteyiz
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var key = "merchantCode"; // controller'daki {merchantCode}'a karşılık gelir. camelcase olmak zorunda
								  // get data

		var merchantCode = context.RouteData.Values[key]; // bu isimde bir key oluşturdu. bu kullanım sayesinde kodu query string yerine path olarak veriyoruz

		// set data

		var baseRequest = context
			.ActionArguments.FirstOrDefault(a => a.Value != null && a.Value.GetType().Equals(typeof(MerchantBaseRequest)));

		if (baseRequest.Value is not null)
		{
			var req = baseRequest.Value as MerchantBaseRequest;

			req.MerchantCode = merchantCode.ToString(); // path olarak verdiysek request'i güncelle
		}

		else
		{
			if (!context.ActionArguments.ContainsKey(key))
			{
				context.ActionArguments.Add(key, merchantCode);
			}

			else
				context.ActionArguments[key] = merchantCode;
		}

		await next();
	}
}
