using Microsoft.AspNetCore.Authorization;

namespace  UMS.Application.CustomHandler
{
    public static class AuthorizationPolicyBuilderExtension
    {
        public static AuthorizationPolicyBuilder UserRequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new CustomUserRequireClaim(claimType));
            return builder;
        }
        public class CustomUserRequireClaim : IAuthorizationRequirement
        {
            public string ClaimType { get; }
            public CustomUserRequireClaim(string claimType)
            {
                ClaimType = claimType;
            }
        }
    }
}

    
