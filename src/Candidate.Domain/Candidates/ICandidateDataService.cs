using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Candidate.Domain.Candidates
{
    public interface ICandidateDataService
    {
        Task<List<CandidateDto>> RetrieveAsync(List<string> skills, CancellationToken cancellationToken);
        Task StoreAsync(CandidateDto candidate, CancellationToken cancellationToken);
    }
}