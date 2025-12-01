using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VisitorFeatures.Session
{
    public class VisitorSession
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string EntranceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string TeamId { get; set; } = string.Empty;
        public bool AcceptedRules { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CheckInTime { get; set; }
        public string VisitorId { get; set; } = string.Empty;
    }
}
