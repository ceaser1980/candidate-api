using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneOf.Types;
using OneOf;

namespace Candidate.Domain.Candidates
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateDatabaseService _candidateDatabaseService;
        
        public CandidateService(ICandidateDatabaseService candidateDatabaseService)
        {
            _candidateDatabaseService = candidateDatabaseService ?? throw new ArgumentNullException(nameof(candidateDatabaseService));
        }
        
        public async Task<OneOf<Candidate, NotFound>> RetrieveCandidatesWithSkills(List<string> skills)
        {
            var candidate = await _candidateDatabaseService.RetrieveAsync(skills);

            if (!candidate.Any())
                return new NotFound();
            
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
            await _candidateDatabaseService.StoreAsync(new CandidateDto
            {
                Id = Guid.NewGuid(),
                Name = candidate.Name,
                Skills = candidate.Skills.Select(skill => new SkillDto(skill)).ToList()
            });
        }
    }
}