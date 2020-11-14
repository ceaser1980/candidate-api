using System;
using System.Linq;
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
        
        public CandidatesController(ICandidateDatabaseService databaseService, ICandidateService candidateService)
        {
            _candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string skills)
        {
            if (skills is null || skills.Length <= 0)
                return BadRequest();
            
            var splitSkills = skills.Split(",").ToList();

            var candidate = await _candidateService.RetrieveCandidatesWithSkills(splitSkills);

            return candidate.Match<IActionResult>(
                Ok, 
                notFound => NotFound());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Domain.Candidates.Candidate candidate)
        {
            if (candidate is null || candidate.Id == Guid.Empty)
                return BadRequest();
            
            await _candidateService.StoreCandidate(candidate);
            
            return Ok();
        }
    }
}