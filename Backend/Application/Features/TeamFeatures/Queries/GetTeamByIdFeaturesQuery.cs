using Application.Interfaces;
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
    public class GetTeamByIdFeaturesQuery : IRequest<Team>
    {
        public string Id { get; set; }
        
        public class GetTeamByIdFeaturesQueryHandler : IRequestHandler<GetTeamByIdFeaturesQuery, Team>
        {
            private readonly IApplicationDbContext _context;

            public GetTeamByIdFeaturesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Team> Handle(GetTeamByIdFeaturesQuery query, CancellationToken cancellationToken)
            {
                return await _context.Teams.FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);
            }
        }
    }
}
