using Microsoft.AspNetCore.Identity;

namespace EzShop.Api.Repository
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user,List<string> roles);
    }
}
