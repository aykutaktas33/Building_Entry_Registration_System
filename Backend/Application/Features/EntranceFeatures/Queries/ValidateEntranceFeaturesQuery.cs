using Application.Extensions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.EntranceFeatures.Queries
{
    public class ValidateEntranceFeaturesQuery : IRequest<bool>
    {
        public string EntranceId { get; set; }

        public class ValidateEntranceFeaturesQueryHandler : IRequestHandler<ValidateEntranceFeaturesQuery, bool>
        {
            private readonly IApplicationDbContext _context;
            public ValidateEntranceFeaturesQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<bool> Handle(ValidateEntranceFeaturesQuery query, CancellationToken cancellationToken)
            {
                if (query == null)
                    return false;

                return await _context.Entrances.FirstOrDefaultAsync(e => e.Id == query.EntranceId && e.IsActive) != null;
            }
        }
    }
}
