using Microsoft.AspNetCore.Mvc;
using ScalableDiff.Application.Models;
using ScalableDiff.Application.Services;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ScalableDiff.Controllers.v1
{
    /// <summary>
    /// Provides endpoints to do a basic diffing functionality process.
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
        /// Sets the left content of the diff within a .
        /// </summary>
        /// <param name="id">The  id of the content.</param>
        /// <param name="content">The left content details.</param>        
        /// <response code="200">Returns ok</response>
        [HttpPost("{id}/Left")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SetLeftContentAsync(Guid id, [FromBody] string content)
        {
            var diffContent = DiffContent.Create(id, content);
            if (await diffAppService.SetLeftDiffContent(diffContent))
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Sets the right content of the diff.
        /// </summary>
        /// <param name="id">The  id of the content.</param>
        /// <param name="content">The right content details.</param>        
        /// <response code="200">Returns ok</response>
        [HttpPost("{id}/Right")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SetRightContentAsync(Guid id, [FromBody] string content)
        {
            var diffContent = DiffContent.Create(id, content);
            if (await diffAppService.SetRightDiffContent(diffContent))
                return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Executes the diff with the provided id.
        /// </summary>
        /// <param name="id">The id of the diff.</param>
        /// <response code="200">Returns the diff result message.</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ExecuteAsync(Guid id)
        {
            var diffSummary = await diffAppService.ExecuteDiff(id);
            if(diffSummary != null)
                return Ok(diffSummary);

            return NotFound();
        }
    }
}
