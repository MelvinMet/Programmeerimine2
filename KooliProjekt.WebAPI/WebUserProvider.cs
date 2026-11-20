using System.Linq;
using System.Security.Claims;
using KooliProjekt.Application.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace KooliProjekt.WebAPI
{
    public class WebUserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebUserProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public int? UserId()
        {
            var value = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(value, out var id) ? id : (int?)null;
        }
    }
}
