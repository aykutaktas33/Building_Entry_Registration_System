using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.VisitorFeatures
{
    public class CheckInResponse
    {
        public string VisitorId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string EntranceId { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public string LocalCheckInTime { get; set; } = string.Empty;
    }
}
