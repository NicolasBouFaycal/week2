using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using UMS.Common.Abstraction;
using UMS.Domain.LinqModels;

namespace UMS.API.Middleware
{
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly MyDbContext _context;
        private static string apikey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";
        private readonly IConfiguration _configuration;


        public TokenExpirationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            // _context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.StartsWithSegments("/api/Authentication/Login"))
            {
                await _next(context);
                return;
            }
            var apiKey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token == null)
            {
                throw new Exception("Null Token ,Enter Token in Authorization Header");
            }
            string[] accesstoken = token.ToString().Split(' ');
            var userid = context.Request.Cookies["UserId"];
            if (!string.IsNullOrEmpty(token))
            {

                HttpClient clientAccessToken = new HttpClient();

                var requestDataAccessToken = new
                {
                    idToken = accesstoken[1]
                };

                var requestUrlCheckAccessToken = $"https://www.googleapis.com/identitytoolkit/v3/relyingparty/getAccountInfo?key={apiKey}";
                var requestContents = new StringContent(JsonConvert.SerializeObject(requestDataAccessToken), Encoding.UTF8, "application/json");

                var responseFromFirebase = await clientAccessToken.PostAsync(requestUrlCheckAccessToken, requestContents);
                var responseContents = await responseFromFirebase.Content.ReadAsStringAsync();
                var responseDataFromFirebase = JsonConvert.DeserializeObject<dynamic>(responseContents);

                if (responseDataFromFirebase.error != null)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Token expired");
                    var refreshToken = context.Request.Headers["Refresh"];
                    var refreshTokenn = refreshToken[0];


                    using (var client = new HttpClient())
                    {
                        var requestData = new
                        {
                            grant_type = "refresh_token",
                            refresh_token = refreshTokenn
                        };

                        var requestUrl = $"https://securetoken.googleapis.com/v1/token?key={apiKey}";
                        var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(requestUrl, requestContent);
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        var newIdToken = responseData.id_token;
                        var newRefreshToken = responseData.refresh_token;
                        context.Response.Cookies.Append("Token", newIdToken);
                        context.Response.Cookies.Append("RefreshToken", newRefreshToken);
                    }
                }
/*
                //to get the firebase user id directly from header request
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("C:/Users/nicol/source/repos/testDDD/UMS.API/firebase-config.json")
                });
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(accesstoken[1], false);
                string userId = decodedToken.Uid;*/

                var uid = responseDataFromFirebase.users[0].localId;
                context.Items["userId"]=uid;
                Uid.uid=uid;

               

                

                var id= ((object)uid).ToString(); ;
                //var roles = (from u in _context.Users join rol in _context.Roles on u.RoleId equals rol.Id where u.KeycloakId == id select rol.Name).FirstOrDefault();
                /*var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, responseDataFromFirebase.users[0].Email),
                    new Claim(ClaimTypes.Role, roles) 
                };*/
                await _next(context);
            }
            else
            {
                await _next(context);
                return;
            }
        }
    }
}

