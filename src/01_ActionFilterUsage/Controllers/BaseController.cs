using Microsoft.AspNetCore.Mvc;

namespace _01_ActionFilterUsage.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
	public int? Page { get; set; }
	public int? PageSize { get; set; }
}
