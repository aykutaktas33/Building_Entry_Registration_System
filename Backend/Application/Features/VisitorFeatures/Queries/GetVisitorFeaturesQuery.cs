using Application.Interfaces;
using Application.Responses.VisitorFeatures;
using Domain.Entities;
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
    public class GetVisitorFeaturesQuery : IRequest<CheckInResponse>
    {
        public string Id { get; set; }
        
        public class GetVisitorFeaturesQueryHandler : IRequestHandler<GetVisitorFeaturesQuery, CheckInResponse>
        {
            private readonly IApplicationDbContext _context;

            public GetVisitorFeaturesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<CheckInResponse> Handle(GetVisitorFeaturesQuery query, CancellationToken cancellationToken)
            {
                var visitor = await _context.Visitors.Include(v => v.TeamId).FirstOrDefaultAsync(v => v.Id == query.Id);

                if (visitor == null)
                {
                    return null;
                }

                var team = await _context.Teams.FindAsync(visitor.TeamId);

                return new CheckInResponse
                {
                    VisitorId = visitor.Id,
                    Name = visitor.Name,
                    Email = visitor.Email,
                    Company = visitor.Company,
                    TeamName = team?.Name ?? "Bilinmiyor",
                    EntranceId = visitor.EntranceId,
                    CheckInTime = visitor.CheckInTime,
                    LocalCheckInTime = visitor.CheckInTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")
                };
            }
        }
    }
}
