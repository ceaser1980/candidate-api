using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OneOf.Types;
using OneOf;

namespace Candidate.Domain.Candidates
{
    ///<inheritdoc/>
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateDataService _candidateDataService;
        private readonly ILogger _logger;
        
        /// <summary>
        /// Constructor for candidate service
        /// </summary>
        /// <param name="candidateDataService">Data service for storing and retrieving candidates</param>
        public CandidateService(ICandidateDataService candidateDataService, ILogger<CandidateService> logger)
        {
            _candidateDataService = candidateDataService ?? throw new ArgumentNullException(nameof(candidateDataService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        ///<inheritdoc/>
        public async Task<OneOf<Candidate, NotFound>> RetrieveCandidateWithSkills(List<string> skills, CancellationToken cancellationToken)
        {
            var candidates = await _candidateDataService.RetrieveAsync(cancellationToken);

            if (!candidates.Any())
            {
                _logger.LogInformation("Unable to find any candidates in the data store");
                return new NotFound();
            }

            var results = MatchCandidateSkills(skills, candidates).ToList();

            if (!results.Any())
            {
                _logger.LogInformation("Unable to find any candidates that match the skills provided");
                return new NotFound();
            }
            
            return results.FirstOrDefault();
        }

        ///<inheritdoc/>
        public async Task StoreCandidate(Candidate candidate, CancellationToken cancellationToken)
        {
            try
            {
                await _candidateDataService.StoreAsync(new CandidateDto
                {
                    Id = Guid.NewGuid(),
                    Name = candidate.Name,
                    Skills = candidate.Skills.Select(skill => new SkillDto(skill)).ToList()
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "Error storing candidate with id {Id} in the data store", candidate.Id);
                throw;
            }
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