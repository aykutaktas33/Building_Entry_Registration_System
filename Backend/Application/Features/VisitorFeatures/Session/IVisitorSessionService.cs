using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VisitorFeatures.Session
{
    public interface IVisitorSessionService
    {
        VisitorSession GetOrCreateSession(string sessionId);
        void UpdateSession(string sessionId, VisitorSession session);
        VisitorSession GetSession(string sessionId);
        void CompleteSession(string sessionId);
    }
}
