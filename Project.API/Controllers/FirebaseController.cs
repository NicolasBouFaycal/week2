using Firebase.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectName.Persistence;
using System.Security.Claims;

namespace Project.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class FirebaseController : Controller
    {
        private static string apikey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";
        public readonly MyDbContext _context;
        public FirebaseController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost(template: "Login")]
        public async Task<ActionResult<string>> Login([FromQuery] string email, [FromQuery] string password)
        {
            FirebaseAuthProvider firebaseAuthProvider=new FirebaseAuthProvider(new FirebaseConfig(apikey));

            try
            {
                FirebaseAuthLink firebaseAuthLink= await firebaseAuthProvider.SignInWithEmailAndPasswordAsync(email, password);
                CookieOptions options = new CookieOptions();
                options.Secure = true;
                Response.Cookies.Append("Token", "Bearer "+firebaseAuthLink.FirebaseToken);
                Response.Cookies.Append("UserId", firebaseAuthLink.User.LocalId);
                var roles = (from u in _context.Users join rol in _context.Roles on u.RoleId equals rol.Id where u.KeycloakId == firebaseAuthLink.User.LocalId select rol.Name).FirstOrDefault();
                var email1 = (from u in _context.Users  where u.KeycloakId == firebaseAuthLink.User.LocalId select u.Email).FirstOrDefault();
                var userClaims = new List<Claim>()
                {
                    new Claim("userId",firebaseAuthLink.User.LocalId),
                    new Claim(ClaimTypes.Email, email1),
                    new Claim(ClaimTypes.Role,roles)
                };
                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                HttpContext.SignInAsync(userPrincipal);

                return firebaseAuthLink.User.LocalId;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
    }
}
