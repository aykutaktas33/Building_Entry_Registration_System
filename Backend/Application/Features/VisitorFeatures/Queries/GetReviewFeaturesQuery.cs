using Application.Features.VisitorFeatures.Session;
using Application.Interfaces;
using Application.Responses.VisitorFeatures;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TeamFeatures.Queries
{
    public class GetReviewFeaturesQuery : IRequest<ReviewResponse>
    {
        public string SessionId { get; set; } = string.Empty;

        public class GetReviewFeaturesQueryHandler : IRequestHandler<GetReviewFeaturesQuery, ReviewResponse>
        {
            private readonly IVisitorSessionService _sessionService;
            private readonly IApplicationDbContext _context;

            public GetReviewFeaturesQueryHandler(IVisitorSessionService sessionService, IApplicationDbContext context)
            {
                _sessionService = sessionService;
                _context = context;
            }

            public async Task<ReviewResponse> Handle(GetReviewFeaturesQuery request, CancellationToken cancellationToken)
            {
                var session = _sessionService.GetSession(request.SessionId);
                if (session == null)
                    throw new BusinessException("Session not found");

                var team = await _context.Teams.FindAsync(session.TeamId);
                var entrance = await _context.Entrances.FindAsync(session.EntranceId);

                // Validate all required data is present
                if (string.IsNullOrEmpty(session.EntranceId) || string.IsNullOrEmpty(session.Name) ||
                    string.IsNullOrEmpty(session.TeamId) || !session.AcceptedRules)
                    throw new BusinessException("Please complete all steps before review");

                return new ReviewResponse
                {
                    SessionId = session.SessionId,
                    EntranceId = session.EntranceId,
                    EntranceName = entrance?.Name ?? "Unknown",
                    Name = session.Name,
                    Email = session.Email,
                    Company = session.Company,
                    TeamId = session.TeamId,
                    TeamName = team?.Name ?? "Unknown",
                    AcceptedRules = session.AcceptedRules,
                    IsReadyForCheckIn = true
                };
            }
        }
    }
}
