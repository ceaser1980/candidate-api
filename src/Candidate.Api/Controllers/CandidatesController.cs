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
        private readonly ICandidateDatabaseService _candidateDatabaseService;
        
        public CandidatesController(ICandidateDatabaseService databaseService)
        {
            _candidateDatabaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] string skills)
        {
            var splitSkills = skills.Split(",");

            var candidates = await _candidateDatabaseService.RetrieveAsync(splitSkills.ToList());

            return Ok(candidates.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]Candidate candidate)
        {
            var skillDto = candidate.Skills
                .Select(skill => new SkillDto {Id = Guid.NewGuid(), Skill = skill})
                .ToList();

            var testCandidate = new CandidateDto
            {
                Id = Guid.NewGuid(),
                Name = candidate.Name,
                Skills = skillDto
            };
            
            await _candidateDatabaseService.StoreAsync(testCandidate);
            return Ok(candidate);
        }
    }
}