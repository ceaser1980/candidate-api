using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Candidate.Domain.Candidates
{
    /// <summary>
    /// Data service for storing and retrieving candidates
    /// </summary>
    public interface ICandidateDataService
    {
        /// <summary>
        /// Retrieve all candidates
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<List<CandidateDto>> RetrieveAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Store a candidate
        /// </summary>
        /// <param name="candidate">Candidate to store</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task StoreAsync(CandidateDto candidate, CancellationToken cancellationToken);
    }
}