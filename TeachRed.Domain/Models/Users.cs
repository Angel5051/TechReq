using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeachRed.Domain.Enum;

namespace TeachRed.Domain.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public static implicit operator ClaimsIdentity(Users v)
        {
            throw new NotImplementedException();
        }
    }
}
