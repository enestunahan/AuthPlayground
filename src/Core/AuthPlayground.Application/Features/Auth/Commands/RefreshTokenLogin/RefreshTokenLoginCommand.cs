using AuthPlayground.Application.Common.Security;
using MediatR;

namespace AuthPlayground.Application.Features.Auth.Commands.RefreshTokenLogin;

public sealed record RefreshTokenLoginCommand(string RefreshToken) : IRequest<TokenDto>;
