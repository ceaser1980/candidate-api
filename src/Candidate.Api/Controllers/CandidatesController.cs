using System;
using System.Linq;
using System.Threading.Tasks;
using Candidate.Domain.Candidates;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CandidatesController : Controller
    {
        private readonly ICandidateService _candidateService;
        
        public CandidatesController(ICandidateDatabaseService databaseService, ICandidateService candidateService)
        {
            _candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string skills)
        {
            var splitSkills = skills.Split(",").ToList();

            var candidate = await _candidateService.RetrieveCandidatesWithSkills(splitSkills);

            return candidate.Match<IActionResult>(
                Ok, 
                notFound => NotFound());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Domain.Candidates.Candidate candidate)
        {
            await _candidateService.StoreCandidate(candidate);
            
            return Ok();
        }
    }
}