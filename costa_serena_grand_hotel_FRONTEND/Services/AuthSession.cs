using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace costa_serena_grand_hotel_FRONTEND.Services
{
    public class AuthSession
    {
        private const string TokenKey = "auth.jwt";
        private readonly IHttpContextAccessor _http;

        public AuthSession(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string? GetToken()
            => _http.HttpContext?.Session.GetString(TokenKey);

        public void SetToken(string token)
            => _http.HttpContext?.Session.SetString(TokenKey, token);

        public void Clear()
            => _http.HttpContext?.Session.Remove(TokenKey);

        public bool IsSignedIn
            => !string.IsNullOrWhiteSpace(GetToken());

        private JwtSecurityToken? ReadJwt()
        {
            var token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public string? GetEmail()
        {
            var jwt = ReadJwt();
            return jwt?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        }

        public string? GetUserId()
        {
            var jwt = ReadJwt();

            return jwt?.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier ||
                c.Type == "nameid" ||
                c.Type == "sub")?.Value;
        }

        public string GetCartOwnerKey()
        {
            if (!IsSignedIn)
                return "anonymous";

            var userId = GetUserId();
            if (!string.IsNullOrWhiteSpace(userId))
                return $"user:{userId}";

            var email = GetEmail();
            if (!string.IsNullOrWhiteSpace(email))
                return $"user:{email.Trim().ToLowerInvariant()}";

            return "authenticated";
        }

        public IReadOnlyList<string> GetRoles()
        {
            var jwt = ReadJwt();

            if (jwt is null)
                return Array.Empty<string>();

            return jwt.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public bool IsInRole(string role)
            => GetRoles().Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
    }
}