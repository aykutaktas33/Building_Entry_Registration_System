using Application.Features.EntranceFeatures.Queries;
using Application.Features.TeamFeatures.Queries;
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
    public class TeamsController : BaseApiController
    {
        /// <summary>
        /// Get Teams
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTeams(int? skip, int? top)
        {
            return Ok(await Mediator.Send(new GetTeamFeaturesQuery(page: skip ?? 1, pageSize: top ?? 25)));
        }

        /// <summary>
        /// Get Team By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await Mediator.Send(new GetTeamByIdFeaturesQuery() { Id = id }));
        }
    }
}