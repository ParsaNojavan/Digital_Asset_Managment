using AuthService.Application.Services.JWT;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AuthService.Application.CQRS.Command.Register.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand,string>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public RegisterUserCommandHandler(IUserRepository userRepository
            , IJwtService jwtService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmail(request.Email);

            if (existingUser != null)
                throw new Exception("User with this email already exists");

            var user = new User
            {
                Username = request.UserName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _userRepository.InsertUser(user);

            var refreshTokenValue = _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            var token = _jwtService.GenerateToken(user);
            return token;
            
        }
    }
}
