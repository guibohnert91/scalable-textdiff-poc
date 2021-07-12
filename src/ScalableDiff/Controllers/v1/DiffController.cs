using Microsoft.AspNetCore.Mvc;
using ScalableDiff.Application.Models;
using ScalableDiff.Application.Services;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ScalableDiff.Controllers.v1
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class DiffController : ControllerBase
    {
        private readonly IDiffAppService diffAppService;

        public DiffController(IDiffAppService diffAppService)
        {
            this.diffAppService = diffAppService;
        }

        /// <summary>
        /// Sets the left content of the diff within a session.
        /// </summary>
        /// <param name="sessionId">The session id of the content.</param>
        /// <param name="content">The left content details.</param>        
        /// <response code="200">Returns ok</response>
        [HttpPost("{sessionId}/Left")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SetLeftContentAsync(Guid sessionId, [FromBody]string content)
        {
            try
            {
                var diffContent = DiffContent.Create(sessionId, content);
                if (await diffAppService.SetLeftDiffContent(diffContent))
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("Something bad ocurred.");
            }
        }

        /// <summary>
        /// Sets the right content of the diff within a session.
        /// </summary>
        /// <param name="sessionId">The session id of the content.</param>
        /// <param name="content">The right content details.</param>        
        /// <response code="200">Returns ok</response>
        [HttpPost("{sessionId}/Right")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SetRightContentAsync(Guid sessionId, [FromBody] string content)
        {
            try
            {
                var diffContent = DiffContent.Create(sessionId, content);
                if(await diffAppService.SetRightDiffContent(diffContent))
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest("Something bad ocurred.");
            }
        }

        /// <summary>
        /// Executes the diff within a session.
        /// </summary>
        /// <param name="sessionId">The session id of the content.</param>
        /// <response code="200">Returns ok</response>
        [HttpGet("{sessionId}")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ExecuteAsync(Guid sessionId)
        {
            try
            {
                return Ok(await diffAppService.ExecuteDiff(sessionId));
            }
            catch (Exception ex)
            {
                return BadRequest("Something bad ocurred.");
            }
        }
    }
}
