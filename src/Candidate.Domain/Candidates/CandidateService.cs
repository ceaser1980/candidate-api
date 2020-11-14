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
            var candidates = await _candidateDataService.RetrieveAsync(cancellationToken);

            if (!candidates.Any())
                return new NotFound();

            return MatchCandidateSkills(skills, candidates).FirstOrDefault();
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
        
        private static IEnumerable<Candidate> MatchCandidateSkills(List<string> skills, IEnumerable<CandidateDto> candidates)
        {
            var matchedCandidates = new List<Candidate>();
                
            foreach (var candidate in candidates)
            {
                var candidateSkills = candidate.Skills
                    .Select(x => x.Skill)
                    .ToArray();

                var matchedCounter = 
                    candidateSkills
                        .Select(skill => skills.Find(x => x == skill))
                        .Count(result => result != null);

                if (matchedCounter > 0)
                {
                    matchedCandidates.Add(new Candidate
                    {
                        Id = candidate.Id,
                        Name = candidate.Name,
                        Skills = candidateSkills
                    });
                }
            }

            return matchedCandidates;
        }
    }
}