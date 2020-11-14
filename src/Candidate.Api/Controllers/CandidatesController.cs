using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Candidate.Domain.Candidates;
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
        /// <param name="skills">Skills to match against the candidates</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Domain.Candidates.Candidate candidate, CancellationToken cancellationToken)
        {
            if (candidate is null || candidate.Id == Guid.Empty)
                return BadRequest();
            
            await _candidateService.StoreCandidate(candidate, cancellationToken);
            
            return Ok();
        }
    }
}