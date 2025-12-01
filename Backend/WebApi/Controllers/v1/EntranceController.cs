using Application.Features.EntranceFeatures.Commands;
using Application.Features.EntranceFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BuildingEntryApi.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class EntranceController : BaseApiController
    {
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateAndSave(ValidateAndSaveEntranceFeaturesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Quick Validate Only (for QR scan preview)
        /// </summary>
        /// <param name="entranceId"></param>
        /// <returns></returns>
        [HttpGet("validate/{entranceId}")]
        public async Task<IActionResult> QuickValidate(string entranceId)
        {
            return Ok(await Mediator.Send(new ValidateEntranceFeaturesQuery() { EntranceId = entranceId }));
        }
    }
}