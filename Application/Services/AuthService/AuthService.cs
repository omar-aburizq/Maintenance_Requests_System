using Application.Repositories;
using Application.Services.AuthService.DTOs;
using Application.Services.CurrentUserService;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Token> _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;

        public AuthService(IGenericRepository<User> userRepository, IGenericRepository<Token> refreshTokenRepository, IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto input)
        {

            var user = await _userRepository.GetAll().Include(x => x.Role).FirstOrDefaultAsync(x => (x.Email.ToLower().Trim() == input.Username.ToLower().Trim() || x.PhoneNumber.Trim() == input.Username.Trim()) && (x.IsActive == true));

            if (user == null)
                throw new Exception("userName or password invalid");


            var passwordHaser = new PasswordHasher<User>(); // Install Microsoft.Extensions.Identity.Core
            var PasswordStatus = passwordHaser.VerifyHashedPassword(user, user.Password, input.Password);

            if (PasswordStatus == PasswordVerificationResult.Failed)
                throw new Exception("userName or password invalid");

            var accessToken = await GenerateAccessToken(user);
            var refershToken = GenerateRefreshToken();

            await _refreshTokenRepository.InsertAsync(new Token  // RefreshToken Entity First
            {
                UserId = user.Id,
                TokenStr = refershToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });
            await _refreshTokenRepository.SaveChangesAsync();

            var result = new LoginResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,
                RoleName = user.Role.Name,
                RoleCode = user.Role.Code,

                AccessToken = accessToken,
                RefershToken = refershToken,
            };
            return result;

        }

        private async Task<string> GenerateAccessToken(User user) // inject IConfiguration First
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"])); // Install Microsoft.IdentityModel.Tokens

            var claims = new List<Claim>  // List Save current User Information  
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim (ClaimTypes.Name , user.Name),
                new Claim (ClaimTypes.Email , user.Email),
                new Claim (ClaimTypes.MobilePhone , user.PhoneNumber),
                new Claim (ClaimTypes.Role , user.Role.Name),
                new Claim("location", user.Location ?? "")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = jwtSection["Issuer"],
                Audience = jwtSection["Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler(); // Install System.IdentityModel.Tokens.Jwt
            var token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[64];
            RandomNumberGenerator.Fill(random);
            return Convert.ToBase64String(random);
        }


        public async Task ChangeUserPassword(ChangeUserPasswordDto input)
        {
            var UserId = _currentUserService.UserId;

            if (UserId == null)
                throw new Exception("User not authenticated");

            var user = await _userRepository.GetByIdAsync(UserId.Value);

            if (user == null)
                throw new Exception("User not found");


            var passwordHasher = new PasswordHasher<User>();
            var PasswordStatus = passwordHasher.VerifyHashedPassword(user, user.Password, input.CurrentPassword);

            if (PasswordStatus == PasswordVerificationResult.Failed)
                throw new Exception("Current Password Invalid");

            if (input.NewPassword != input.ConfirmNewPassword)
                throw new Exception("Confirm Password Not Matches");

            user.Password = passwordHasher.HashPassword(user, input.NewPassword);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }


        public async Task<string> RefreshToken(RefreshTokenDto input)
        {
            var refreshToken = await _refreshTokenRepository.GetAll().Include(x => x.User).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.TokenStr == input.TokenStr && x.ExpiryDate > DateTime.UtcNow);

            if (refreshToken == null)
                throw new Exception("Invalid Refresh Token");

            var accessToken = await GenerateAccessToken(refreshToken.User);

            return accessToken;
        }

        public async Task Logout(RefreshTokenDto input)
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                throw new Exception("User not authenticated");


            var refreshToken = await _refreshTokenRepository.GetAll().FirstOrDefaultAsync(x => x.TokenStr == input.TokenStr && x.UserId == userId);

            if (refreshToken == null)
                throw new Exception("Refresh Token Not Found");

            _refreshTokenRepository.Delete(refreshToken);

            await _refreshTokenRepository.SaveChangesAsync();
        }
    }
}
