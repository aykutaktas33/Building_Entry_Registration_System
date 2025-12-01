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
    public class GetTeamFeaturesQuery : IRequest<IEnumerable<Team>>
    {
        public long Page { get; set; } // Current page number
        public long PageSize { get; set; } // Number of items per page

        public GetTeamFeaturesQuery(long page = 1, long pageSize = 25)
        {
            Page = page;
            PageSize = pageSize;
        }

        public class GetTeamFeaturesQueryHandler : IRequestHandler<GetTeamFeaturesQuery, IEnumerable<Team>>
        {
            private readonly IApplicationDbContext _context;

            public GetTeamFeaturesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Team>> Handle(GetTeamFeaturesQuery query, CancellationToken cancellationToken)
            {
                // Calculate skip count based on pagination parameters
                var skipCount = (query.Page - 1) * query.PageSize;

                // Retrieve users with pagination
                var list = await _context.Teams
                    .Where(u => !u.IsDelete) // Ensure soft-deleted users are excluded
                    .Skip((int)skipCount)
                    .Take((int)query.PageSize)
                    .ToListAsync(cancellationToken);

                if (list == null)
                {
                    return null;
                }

                return list.AsReadOnly();
            }
        }
    }
}
