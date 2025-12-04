using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeachRed.Domain.Models;

namespace TeachRed.Domain.Helpers
{
    public static class AuthenticateUserHelper
    {
        public static ClaimsIdentity Authenticate(Users user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),

        };
            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimTypes.Email, ClaimsIdentity.DefaultRoleClaimType);
        }

    }
}
