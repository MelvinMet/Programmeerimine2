using System.Security.Claims;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Authentication;
using KooliProjekt.Application.Authentication.Jwt;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Authentication
{
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, OperationResult<string>>
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtTokenBuilder _tokens;

        public AuthenticateCommandHandler(ApplicationDbContext db, JwtTokenBuilder tokens)
        {
            _db = db;
            _tokens = tokens;
        }

        public async Task<OperationResult<string>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.UserName, cancellationToken);
            if (user == null || !PasswordHelper.Verify(request.Password, user.Password))
            {
                var error = new OperationResult<string>();
                error.AddError("Invalid e-mail or password.");
                return error;
            }

            var claims = new System.Collections.Generic.List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };
            if (user.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            return new OperationResult<string>(_tokens.GenerateToken(claims.ToArray()));
        }
    }
}
