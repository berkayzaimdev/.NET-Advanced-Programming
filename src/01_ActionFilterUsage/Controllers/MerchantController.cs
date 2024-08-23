using _01_ActionFilterUsage.ActionFilters;
using _01_ActionFilterUsage.Models;
using Microsoft.AspNetCore.Mvc;

namespace _01_ActionFilterUsage.Controllers;
[Route("api/{merchantCode}/[controller]")]
[ApiController]
// [MerchantCodeActionFilter]
public class MerchantController : BaseController
{
	[HttpGet]
	[Route("GetUsers")]
	public IActionResult GetUsers(string merchantCode)
	{
		return Ok($"Users returned for Merchant {merchantCode}");
	}

	[HttpPost]
	[Route("Update")]
	public IActionResult UpdateMerchant(UpdateMerchantRequestModel reqModel)
	{
		return Ok($"Merchant updated. name= {reqModel.Name}, code = {reqModel.MerchantCode}");
	}
}
