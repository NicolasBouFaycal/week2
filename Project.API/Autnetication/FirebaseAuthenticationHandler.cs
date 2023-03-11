using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Project.API.Autnetication
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FirebaseApp _firebaseApp;
        public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, FirebaseApp firebase , ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _firebaseApp= firebase;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Context.Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }
            string bearerToken = Context.Request.Headers["Authentication"];
            if (string.IsNullOrEmpty(bearerToken))
            {
                return AuthenticateResult.Fail("invalid skim");
            }
            string token=bearerToken.Substring("Bearer ".Length);
            FirebaseToken firebaseToken =await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);

            return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(ToClaims(firebaseToken.Claims))
            }),JwtBearerDefaults.AuthenticationScheme));
        }

        private IEnumerable<Claim>? ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {

            };
        }
    }
}
