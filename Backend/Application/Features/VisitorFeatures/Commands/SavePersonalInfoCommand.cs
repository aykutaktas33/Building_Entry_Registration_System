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
    public class SavePersonalInfoCommand : IRequest<VisitorSessionResponse>
    {
        public string SessionId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;


        public class SavePersonalInfoCommandHandler : IRequestHandler<SavePersonalInfoCommand, VisitorSessionResponse>
        {
            private readonly IVisitorSessionService _sessionService;
            public SavePersonalInfoCommandHandler(IVisitorSessionService sessionService)
            {
                _sessionService = sessionService;
            }
            public async Task<VisitorSessionResponse> Handle(SavePersonalInfoCommand command, CancellationToken cancellationToken)
            {
                var session = _sessionService.GetOrCreateSession(command.SessionId);
                session.Name = command.Name;
                session.Email = command.Email;
                session.Company = command.Company;
                _sessionService.UpdateSession(session.SessionId, session);

                return new VisitorSessionResponse
                {
                    SessionId = session.SessionId,
                    NextStep = "select-team",
                    Success = true
                };
            }
        }
    }
}
