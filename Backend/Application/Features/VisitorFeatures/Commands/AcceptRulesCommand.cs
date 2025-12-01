using Application.Features.VisitorFeatures.Session;
using Application.Interfaces;
using Application.Responses.VisitorFeatures;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.VisitorFeatures.Commands
{
    public class AcceptRulesCommand : IRequest<VisitorSessionResponse>
    {
        public string SessionId { get; set; } = string.Empty;
        public bool Accepted { get; set; }


        public class AcceptRulesCommandHandler : IRequestHandler<AcceptRulesCommand, VisitorSessionResponse>
        {
            private readonly IVisitorSessionService _sessionService;

            public AcceptRulesCommandHandler(IVisitorSessionService sessionService)
            {
                _sessionService = sessionService;
            }

            public Task<VisitorSessionResponse> Handle(AcceptRulesCommand request, CancellationToken cancellationToken)
            {
                if (!request.Accepted)
                    throw new BusinessException("You must accept the rules");

                var session = _sessionService.GetSession(request.SessionId);
                if (session == null)
                    throw new BusinessException("Session not found");

                session.AcceptedRules = true;
                _sessionService.UpdateSession(session.SessionId, session);

                return Task.FromResult(new VisitorSessionResponse
                {
                    SessionId = session.SessionId,
                    NextStep = "review"
                });
            }
        }
    }
}
