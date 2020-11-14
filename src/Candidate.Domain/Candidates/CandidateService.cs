using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Candidate.Domain.Candidates
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateDatabaseService _candidateDatabaseService;
        
        public CandidateService(ICandidateDatabaseService candidateDatabaseService)
        {
            _candidateDatabaseService = candidateDatabaseService ?? throw new ArgumentNullException(nameof(candidateDatabaseService));
        }
        
        public async Task<Candidate> RetrieveCandidatesWithSkills(List<string> skills)
        {
            var candidate = await _candidateDatabaseService.RetrieveAsync(skills);

            if (!candidate.Any())
                return null;
            
            return candidate
                .Select(x => new Candidate
                {
                    Id = x.Id,
                    Name = x.Name,
                    Skills = x.Skills
                        .Select(y => y.Skill)
                        .ToArray()
                })
                .OrderByDescending(x => x.Skills.Length)
                .FirstOrDefault();
        }

        public async Task StoreCandidate(Candidate candidate)
        {
            var skillDto = candidate.Skills
                .Select(skill => new SkillDto {Id = Guid.NewGuid(), Skill = skill})
                .ToList();

            await _candidateDatabaseService.StoreAsync(new CandidateDto
            {
                Id = Guid.NewGuid(),
                Name = candidate.Name,
                Skills = skillDto
            });
        }
    }
}