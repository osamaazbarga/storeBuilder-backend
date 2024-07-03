using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace superecommere
{
    public static class SD
    {
        public const string Facebook = "facebook";
        public const string Google = "google";

        //roles
        public const string AdminRole = "Admin";
        public const string ManagerRole = "Manager";
        public const string CustomerRole = "Customer";

        public const string AdminUserName = "admin@test.com";
        public const string SuperAdminChangeNotAllow = "super admin change is not allow!";
        public const int MaximumLoginAttempts = 3;



        public static bool VIPPolicy(AuthorizationHandlerContext context)
        {
            if (context.User.IsInRole(CustomerRole) && context.User.HasClaim(c=>c.Type==ClaimTypes.Email && c.Value.Contains("vip"))) 
            {
                return true;
            }
            return false;
        }


    }
}
