using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;


namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public AccountController(IUserService userService,
                                 ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();

                var user = await _userService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to retrieve the user. Erro: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(UserDto userDto)
        {
            try
            {
                if (await _userService.UserExists(userDto.UserName))
                    return BadRequest("user already exists!");
                
                var user = await _userService.CreateAccountAsync(userDto);

                if (user != null)
                    return Ok("User register with sucess");
                
                return BadRequest("user cannot be created, try again later");

            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to retrieve the user. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            try
            {
               var user = await _userService.GetUserByUserNameAsync(userLoginDto.Username);
               if(user == null) return Unauthorized("User or password not valid!");

               var result = await _userService.CheckUserPasswordAsync(user, userLoginDto.Password);

               if(!result.Succeeded) return Unauthorized();

               return Ok(new 
               {
                userName = user.UserName,
                PrimeiroNome = user.PrimeiroNome,
                token = _tokenService.CreateToken(user).Result
               });
            }
            catch (Exception ex)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Error to retrieve the user. Erro: {ex.Message}");
            }
        }
    }
}