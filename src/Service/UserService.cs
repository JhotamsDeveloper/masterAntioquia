using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Model.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAll();
        Task<UserDto> GetByIdString(string id);
    }

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager,
                           IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAll()
        {

            var _UserList = _userManager.Users;
            return (await _UserList.ToListAsync());
        }

        public async Task<UserDto> GetByIdString(string id)
        {

            return _mapper.Map<UserDto>(
                await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id)
                );

        }

    }
}
