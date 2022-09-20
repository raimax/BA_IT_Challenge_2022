using API.Models;

namespace API.IService
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser appUser);
    }
}
