using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VisitorFeatures.Session
{
    public class VisitorSessionService : IVisitorSessionService
    {
        private static readonly ConcurrentDictionary<string, VisitorSession> _sessions = new();

        public VisitorSession GetOrCreateSession(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                sessionId = Guid.NewGuid().ToString();

            return _sessions.GetOrAdd(sessionId, new VisitorSession { SessionId = sessionId });
        }

        public VisitorSession GetSession(string sessionId)
        {
            _sessions.TryGetValue(sessionId, out var session);
            return session;
        }

        public void UpdateSession(string sessionId, VisitorSession session)
        {
            _sessions.AddOrUpdate(sessionId, session, (key, oldValue) => session);
        }

        public void CompleteSession(string sessionId)
        {
            _sessions.TryRemove(sessionId, out _);
        }
    }
}
