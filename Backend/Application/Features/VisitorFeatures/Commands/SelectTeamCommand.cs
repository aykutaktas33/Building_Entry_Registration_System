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
    public class SelectTeamCommand : IRequest<VisitorSessionResponse>
    {
        public string SessionId { get; set; } = string.Empty;
        public string TeamId { get; set; } = string.Empty;


        public class SelectTeamCommandHandler : IRequestHandler<SelectTeamCommand, VisitorSessionResponse>
        {
            private readonly IVisitorSessionService _sessionService;
            private readonly IApplicationDbContext _context;

            public SelectTeamCommandHandler(IVisitorSessionService sessionService, IApplicationDbContext context)
            {
                _sessionService = sessionService;
                _context = context;
            }
            public async Task<VisitorSessionResponse> Handle(SelectTeamCommand command, CancellationToken cancellationToken)
            {
                var team = await _context.Teams.FindAsync(command.TeamId);
                if (team == null)
                    throw new BusinessException("Invalid team");

                var session = _sessionService.GetSession(command.SessionId);
                if (session == null)
                    throw new BusinessException("Session not found");

                session.TeamId = command.TeamId;
                _sessionService.UpdateSession(session.SessionId, session);

                return new VisitorSessionResponse
                {
                    SessionId = session.SessionId,
                    NextStep = "accept-rules"
                };
            }
        }
    }
}
