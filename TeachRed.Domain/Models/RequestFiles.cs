// RequestFiles.cs
using System;

namespace TechReq.Domain.Models
{
    public class RequestFiles
    {
        public Guid Id { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int RequestId { get; set; }
    }
}