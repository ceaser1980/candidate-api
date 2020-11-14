using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Candidate.Domain.Candidates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;
        
        /// <summary>
        /// Constructor for candidates controller
        /// </summary>
        /// <param name="candidateService">Service for handling candidates</param>
        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        }
        
        /// <summary>
        /// Get method for retrieving candidates based on provided skill set
        /// </summary>
        /// <param name="skills">Skills to match against the candidates as a comma delimited string</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <response code="200">Returns an OK response with the matched candidate</response>
        /// <response code="400">Returns if no skills are provided or the skills can not be parsed</response>
        /// <response code="404">Returns if no candidates exist or one can not be matched with the skills provided</response> 
        [HttpGet]
        [ProducesResponseType(typeof(Domain.Candidates.Candidate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAsync([FromQuery] string skills, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(skills))
                return BadRequest();
            
            var splitSkills = skills.Split(",").ToList();

            if (!splitSkills.Any())
                return BadRequest();

            var candidate = await _candidateService.RetrieveCandidateWithSkills(splitSkills, cancellationToken);

            return candidate.Match<IActionResult>(
                Ok, 
                notFound => NotFound());
        }

        /// <summary>
        /// Post method for storing candidates
        /// </summary>
        /// <param name="candidate">Candidate to store</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <response code="200">Returns an OK response when the candidate has been stored</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> PostAsync([FromBody]Domain.Candidates.Candidate candidate, CancellationToken cancellationToken)
        {
            if (candidate is null || candidate.Id == Guid.Empty)
                return BadRequest();
            
            await _candidateService.StoreCandidate(candidate, cancellationToken);
            
            return Ok();
        }
    }
}