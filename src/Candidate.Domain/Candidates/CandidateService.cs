using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OneOf.Types;
using OneOf;

namespace Candidate.Domain.Candidates
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateDataService _candidateDataService;
        
        public CandidateService(ICandidateDataService candidateDataService)
        {
            _candidateDataService = candidateDataService ?? throw new ArgumentNullException(nameof(candidateDataService));
        }
        
        public async Task<OneOf<Candidate, NotFound>> RetrieveCandidatesWithSkills(List<string> skills, CancellationToken cancellationToken)
        {
            var candidate = await _candidateDataService.RetrieveAsync(skills, cancellationToken);

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

        public async Task StoreCandidate(Candidate candidate, CancellationToken cancellationToken)
        {
            await _candidateDataService.StoreAsync(new CandidateDto
            {
                Id = Guid.NewGuid(),
                Name = candidate.Name,
                Skills = candidate.Skills.Select(skill => new SkillDto(skill)).ToList()
            }, cancellationToken);
        }
    }
}