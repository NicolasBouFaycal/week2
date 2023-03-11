using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseController : Controller
    {
        private static string apikey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";

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
                return firebaseAuthLink.User.LocalId;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }
    }
}
