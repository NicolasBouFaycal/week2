using Firebase.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

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
            //_context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.StartsWithSegments("/api/Firebase/Login"))
            {
                await _next(context);
                return;
            }
            var token = context.Request.Cookies["Token"];
            var userid = context.Request.Cookies["UserId"];
            if (token != null)
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var expires = jwtToken.ValidTo;
                if (expires < DateTime.UtcNow)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("Token expired");
                    var refreshToken = context.Request.Cookies["RefreshToken"];
                    var refreshTokenn = refreshToken;
                    var apiKey = "AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI";

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
            }
            await _next(context);
        }
    }
}

