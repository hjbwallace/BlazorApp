using Microsoft.AspNetCore.Authorization;

namespace BlazorApp
{
    public static class Policies
    {
        public const string IsAdmin = nameof(IsAdmin);

        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Admin")
                                                   .Build();
        }
    }
}