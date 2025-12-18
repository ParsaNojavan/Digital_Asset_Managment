using AuthService.Application.DTOs.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.CQRS.Command.Refresh
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
