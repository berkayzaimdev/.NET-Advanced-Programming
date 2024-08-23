namespace _01_ActionFilterUsage.Models;

public class UpdateMerchantRequestModel : MerchantBaseRequest
{
	public string Name { get; set; }
}

public class MerchantBaseRequest
{
	public string MerchantCode { get; set; }
}