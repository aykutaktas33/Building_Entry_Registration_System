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

namespace Application.Features.EntranceFeatures.Commands
{
    public class ValidateAndSaveEntranceFeaturesCommand : IRequest<VisitorSessionResponse>
    {
        public string SessionId { get; set; } = string.Empty;
        public string EntranceId { get; set; } = string.Empty;


        public class ValidateAndSaveEntranceFeaturesCommandHandler : IRequestHandler<ValidateAndSaveEntranceFeaturesCommand, VisitorSessionResponse>
        {
            private readonly IVisitorSessionService _sessionService;
            private readonly IApplicationDbContext _context;

            public ValidateAndSaveEntranceFeaturesCommandHandler(IVisitorSessionService sessionService, IApplicationDbContext context)
            {
                _sessionService = sessionService;
                _context = context;
            }

            public async Task<VisitorSessionResponse> Handle(ValidateAndSaveEntranceFeaturesCommand request, CancellationToken cancellationToken)
            {
                // Validate entrance
                var entrance = await _context.Entrances.FirstOrDefaultAsync(e => e.Id == request.EntranceId && e.IsActive, cancellationToken);

                if (entrance == null)
                    throw new BusinessException("Invalid entrance ID");

                // Get or create session
                var session = _sessionService.GetOrCreateSession(request.SessionId);

                // Save entrance ID to session
                session.EntranceId = request.EntranceId;
                _sessionService.UpdateSession(session.SessionId, session);

                return new VisitorSessionResponse
                {
                    SessionId = session.SessionId,
                    NextStep = "personal-info",
                    Success = true
                };
            }
        }
    }
}
