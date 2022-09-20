using API.Dtos;
using API.Exceptions;
using API.Extensions;
using API.IService;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Service
{
    /// <summary>
    /// Handles business logic related to users
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="authDto"></param>
        /// <returns>AppUser entity</returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<AppUser> CreateAsync(AuthRequestDto authDto)
        {
            AppUser? existingUser = await _userManager.FindByNameAsync(authDto.Username);

            if (existingUser is not null) throw new BadRequestException("Username already exists");

            AppUser user = _mapper.Map<AppUser>(authDto);

            var creationResult = await _userManager.CreateAsync(user, authDto.Password);
            string creationErrors = "";

            foreach (var (error, index) in creationResult.Errors.WithIndex())
            {
                creationErrors += error.Description;
                if (index != creationResult.Errors.Count() - 1)
                {
                    creationErrors += ", ";
                }
            }

            if (!creationResult.Succeeded) throw new BadRequestException(creationErrors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Regular");
            if (!roleResult.Succeeded) throw new BadRequestException(string.Join(", ", roleResult.Errors));

            return user;
        }

        /// <summary>
        /// Finds user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>AppUser entity or null if not found</returns>
        public async Task<AppUser?> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }
}
