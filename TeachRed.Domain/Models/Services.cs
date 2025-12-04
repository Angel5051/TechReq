// Services.cs
using System;

namespace TechReq.Domain.Models
{
    public class Services
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

    }
}