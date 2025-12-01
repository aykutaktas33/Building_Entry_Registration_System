using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.VisitorFeatures
{
    public class VisitorSessionResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public string NextStep { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
    }
}
