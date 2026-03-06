using ITI_MVC_Project.Models.Entities;

namespace ITI_MVC_Project.Authentication_Contract
{
	public interface IJwtProvider
	{
		(string token, int expireTime) GenerateToken(ApplicationUser user);
		string? ValidateToken(string token);
	}
}
