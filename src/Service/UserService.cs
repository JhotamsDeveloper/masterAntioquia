using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


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
