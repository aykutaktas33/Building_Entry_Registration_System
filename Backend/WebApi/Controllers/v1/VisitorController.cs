using Application.Features.TeamFeatures.Queries;
using Application.Features.VisitorFeatures.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingEntryApi.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class VisitorController : BaseApiController
    {
        /// <summary>
        /// Save Personal Information
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("save-personal-info")]
        public async Task<IActionResult> SavePersonalInfo(SavePersonalInfoCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Select Team
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("select-team")]
        public async Task<IActionResult> SelectTeam(SelectTeamCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Accept Rules
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("accept-rules")]
        public async Task<IActionResult> AcceptRules(AcceptRulesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Review
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        [HttpGet("{sessionId}/review")]
        public async Task<IActionResult> Review(string sessionId)
        {
            return Ok(await Mediator.Send(new GetReviewFeaturesQuery { SessionId = sessionId }));
        }

        /// <summary>
        /// CheckIn
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn(CheckInFeaturesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Get Visitor
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitor(string id)
        {
            return Ok(await Mediator.Send(new GetVisitorFeaturesQuery() { Id = id }));
        }
    }
}