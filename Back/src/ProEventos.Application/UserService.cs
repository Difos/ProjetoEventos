using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Dtos;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Infra.Interfaces;


namespace ProEventos.Application
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IMapper mapper,
                           IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                 .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName);

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($" Error trying to verify password. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);

                    return userToReturn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($" Error trying to create account. Erro: {ex.Message}");
            }

            return null;
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(username);

                if (user == null) return null;

                var userUpldateDto = _mapper.Map<UserUpdateDto>(user);

                return userUpldateDto;
            }
            catch (Exception ex)
            {
                throw new Exception($" Error trying to verify if the user exists. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userRepository.GetUserByUserNameAsync(userUpdateDto.UserName);
                if(user == null) return null;

                _mapper.Map(userUpdateDto, user);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);

                _userRepository.Update<User>(user);

                if(await _userRepository.SaveChangesAsync())
                {
                    var userRetorno = await _userRepository.GetUserByUserNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($" Error trying to update a password. Erro: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($" Error trying to verify a user. Erro: {ex.Message}");
            }
        }

    }
}