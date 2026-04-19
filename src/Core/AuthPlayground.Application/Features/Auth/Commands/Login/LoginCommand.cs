using AuthPlayground.Application.Common.Security;
using MediatR;

namespace AuthPlayground.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string UserNameOrEmail, string Password) : IRequest<TokenDto>;
