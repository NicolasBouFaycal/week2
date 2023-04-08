using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using UMS.Domain.Models;
using UMS.Persistence;

namespace UMS.API.Middleware
{
    public class TokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly MyDbContext _context;
        private static string apikey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";

        public TokenExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
           // _context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.StartsWithSegments("/api/Firebase/Login"))
            {
                await _next(context);
                return;
            }
            var apiKey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
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

