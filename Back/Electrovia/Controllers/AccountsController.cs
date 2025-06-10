using AutoMapper;
using Electrovia.DTOs;
using Electrovia.Errors;
using Electrovia.Extentions;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Electrovia_Core.Services;
using Microsoft.AspNetCore.Identity;
using Electrovia_Core.Entities.identity;
using Microsoft.AspNetCore.Authorization;

namespace Electrovia.Controllers
{
    public class AccountsController : BaseApiController
    {
        #region Constructor 
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenservices;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenServices tokenServices, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenservices = tokenServices;
            _mapper = mapper;
        }

        #endregion

        #region Login  
        [HttpPost("login")] // api/Accounts/login
        public async Task<ActionResult<UserDTO?>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email!);
            if (user == null) return Unauthorized(new APIsResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password!, false);
            if (!result.Succeeded) return Unauthorized(new APIsResponse(401));
            return Ok(new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                token = await _tokenservices.CreateToken(user, _userManager)
            });
        }
        #endregion

        #region Register

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO?>> Register(RegisterDTO registerDTO)
        {
            if(Validation_Dublication_Email(registerDTO.Email!).Result.Value)
                return BadRequest(new API_ValidationError_Response() { Errors = new string[] { "This Email is Already Exist" } });

            var user = new AppUser()
            {
                DisplayName = registerDTO?.DisplayName,
                Email = registerDTO?.Email,
                PhoneNumber = registerDTO?.phone,
                UserName = registerDTO?.Email?.Split("@")[0]
            };
            var result = await _userManager.CreateAsync(user, registerDTO!.Password!);
            if (!result.Succeeded) return BadRequest(new APIsResponse(400));
            return Ok(new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                token = await _tokenservices.CreateToken(user, _userManager)
            });
        }

        #endregion

        #region Get_Current_user 
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO?>> Get_Current_User()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email!);
            return Ok(new UserDTO()
            {
                DisplayName = user?.DisplayName,
                Email = user?.Email,
                token = await _tokenservices.CreateToken(user!, _userManager)
            });
        }

        #endregion

        #region Get_Address
        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<AddressDTO?>> Get_Address()
        {
            var user = await _userManager.Find_address_Async(User);
            return Ok(_mapper.Map<Address, AddressDTO>(user!.Address!));
        }

        #endregion

        #region Update_Address
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> Update_Address(AddressDTO update_address)
        {
            var user = await _userManager.Find_address_Async(User);
            var address = _mapper.Map<AddressDTO, Address>(update_address);

            user!.Address!.FirstName = address.FirstName;
            user.Address.LastName = address.LastName;
            user.Address.Street = address.Street;
            user.Address.City = address.City;
            user.Address.Country = address.Country;

            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded) return BadRequest(new APIsResponse(400));
            return Ok(update_address);
        }

        #endregion

        #region Validation Dublicate Email
        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> Validation_Dublication_Email(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
        #endregion
    
    }
}
