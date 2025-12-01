using Application.Extensions;
using Application.Features.VisitorFeatures.Session;
using Application.Interfaces;
using Application.Responses.VisitorFeatures;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.VisitorFeatures.Commands
{
    public class CheckInFeaturesCommand : IRequest<CheckInResponse>
    {
        public string SessionId { get; set; } = string.Empty;

        public class CheckInFeaturesCommandHandler : IRequestHandler<CheckInFeaturesCommand, CheckInResponse>
        {
            private readonly IApplicationDbContext _context;
            private readonly IVisitorSessionService _sessionService;

            public CheckInFeaturesCommandHandler(IApplicationDbContext context, IVisitorSessionService sessionService)
            {
                _context = context;
                _sessionService = sessionService;
            }

            public async Task<CheckInResponse> Handle(CheckInFeaturesCommand command, CancellationToken cancellationToken)
            {
                // Get session data
                var session = _sessionService.GetSession(command.SessionId);
                if (session == null)
                    throw new BusinessException("Session not found");

                // Validate session has all required data
                if (string.IsNullOrEmpty(session.EntranceId) || string.IsNullOrEmpty(session.Name) ||
                    string.IsNullOrEmpty(session.TeamId) || !session.AcceptedRules)
                    throw new BusinessException("Please complete all steps before check-in");

                // Validate entrance
                var entrance = await _context.Entrances.FirstOrDefaultAsync(e => e.Id == session.EntranceId && e.IsActive);
                if (entrance == null)
                    throw new BusinessException("Invalid entrance ID");

                // Get team
                var team = await _context.Teams.FindAsync(session.TeamId);
                if (team == null)
                    throw new BusinessException("Invalid team");

                // Create visitor
                var visitor = new Visitor
                {
                    Name = session.Name,
                    Email = session.Email,
                    Company = session.Company,
                    TeamId = session.TeamId,
                    EntranceId = session.EntranceId,
                    AcceptedRules = session.AcceptedRules,
                    CheckInTime = DateTime.UtcNow
                };

                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync(cancellationToken);

                // Update session with visitor ID and complete
                session.VisitorId = visitor.Id;
                session.CheckInTime = visitor.CheckInTime;
                _sessionService.CompleteSession(session.SessionId);

                return new CheckInResponse
                {
                    VisitorId = visitor.Id,
                    SessionId = session.SessionId,
                    Name = visitor.Name,
                    Email = visitor.Email,
                    Company = visitor.Company,
                    TeamName = team.Name,
                    EntranceId = visitor.EntranceId,
                    CheckInTime = visitor.CheckInTime,
                    LocalCheckInTime = visitor.CheckInTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
                };
            }
        }

    }
}
