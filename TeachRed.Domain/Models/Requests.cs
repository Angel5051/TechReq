// Requests.cs
using System;
using TeachRed.Domain.Enum;

namespace TechReq.Domain.Models
{
    public class Requests
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public StatusRequest Status { get; set; }
        public Guid? AssignedStaffId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }

    }
}