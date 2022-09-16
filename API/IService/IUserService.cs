using API.Dtos;
using API.Models;

namespace API.IService
{
    public interface IUserService
    {
        Task<AppUser> CreateAsync(AuthRequestDto authDto);
        Task<AppUser?> FindByUsernameAsync(string username);
    }
}
