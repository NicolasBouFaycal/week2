using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UMS.Application.Abstraction;
using UMS.Persistence;

namespace UMS.Application.Service
{
    public class FirebasesHelper : IFirebaseHelper
    {
        private static string apikey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";
        public readonly MyDbContext _context;
        public FirebasesHelper(MyDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult<string>> Login(ControllerBase controllerBase,[FromQuery] string email, [FromQuery] string password)
        {
            FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(apikey));

            try
            {
                FirebaseAuthLink firebaseAuthLink = await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                //CookieOptions options = new CookieOptions();
                //options.Secure = true;
                controllerBase.Response.Cookies.Append("Token",firebaseAuthLink.FirebaseToken);
                controllerBase.Response.Cookies.Append("RefreshToken",firebaseAuthLink.RefreshToken);
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(firebaseAuthLink.FirebaseToken);

                

                /*var exp = jwtToken.ValidTo;
                if (exp < DateTime.UtcNow)
                {
                    // Token has expired, return an error response
                    return controllerBase.Unauthorized();
                }*/
                //var client = new HttpClient();
                // var token = firebaseAuthLink.FirebaseToken;
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // controllerBase.Response.Cookies.Append("UserId", firebaseAuthLink.User.LocalId);


                controllerBase.Response.Cookies.Append("UserId", firebaseAuthLink.User.LocalId);

                var roles = (from u in _context.Users join rol in _context.Roles on u.RoleId equals rol.Id where u.KeycloakId == firebaseAuthLink.User.LocalId select rol.Name).FirstOrDefault();
                var userClaims = new List<Claim>()
                {
                    new Claim("userId",firebaseAuthLink.User.LocalId),
                    new Claim(ClaimTypes.Email, firebaseAuthLink.User.Email),
                    new Claim(ClaimTypes.Role,roles)
                };
                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                //await controllerBase.HttpContext.SignInAsync(userPrincipal);

                return "Access Token : "+firebaseAuthLink.FirebaseToken +"\n "+ "Refresh Token : "+firebaseAuthLink.RefreshToken;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not found");

            }
        }
    }
}
