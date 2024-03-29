using LOAN_API.Models.DTO;
using LOAN_API.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using LOAN_API.Data;
using Microsoft.Extensions.Configuration;

namespace LOAN_API.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(UserDto userDto);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserDbContext _context;

        public AuthenticationService(UserDbContext context)
        {
            _context = context;
        }
        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                {
                    throw new Exception("Incorrect UserName or Password");
                }

                var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new("JWTID", Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, user.Role),
            };

                return GenerateNewJsonWebToken(authClaims);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        private static string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()["AppSettings:Secret"]));


            var tokenObject = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }
        public async Task RegisterAsync(UserDto userDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    throw new Exception("Email is already registered");
                }

                if (await _context.Users.AnyAsync(u => u.UserName == userDto.UserName))
                {
                    throw new Exception("Username is already registered");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var newUser = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    UserName = userDto.UserName,
                    Age = userDto.Age,
                    Income = userDto.Income,
                    Email = userDto.Email,
                    Password = hashedPassword,
                    Role = Role.User
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
