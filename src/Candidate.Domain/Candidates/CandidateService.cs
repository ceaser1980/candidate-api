using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OneOf.Types;
using OneOf;

namespace Candidate.Domain.Candidates
{
    ///<inheritdoc/>
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateDataService _candidateDataService;
        
        /// <summary>
        /// Constructor for candidate service
        /// </summary>
        /// <param name="candidateDataService">Data service for storing and retrieving candidates</param>
        public CandidateService(ICandidateDataService candidateDataService)
        {
            _candidateDataService = candidateDataService ?? throw new ArgumentNullException(nameof(candidateDataService));
        }
        
        ///<inheritdoc/>
        public async Task<OneOf<Candidate, NotFound>> RetrieveCandidateWithSkills(List<string> skills, CancellationToken cancellationToken)
        {
            var candidates = await _candidateDataService.RetrieveAsync(cancellationToken);

            if (!candidates.Any())
                return new NotFound();

            return MatchCandidateSkills(skills, candidates).FirstOrDefault();
        }

        ///<inheritdoc/>
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