using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OneOf.Types;
using OneOf;

namespace Candidate.Domain.Candidates
{
    /// <summary>
    /// Domain service for handling candidates
    /// </summary>
    public interface ICandidateService
    {
        /// <summary>
        /// Retrieve a candidate with closest match to skills provided
        /// </summary>
        /// <param name="skills">Skills that should be matched against candidates</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<OneOf<Candidate, NotFound>> RetrieveCandidateWithSkills(List<string> skills, CancellationToken cancellationToken);

        /// <summary>
        /// Store a candidate
        /// </summary>
        /// <param name="candidate">Candidate to store</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task StoreCandidate(Candidate candidate, CancellationToken cancellationToken);
    }
}