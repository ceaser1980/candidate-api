using System.Collections.Generic;
using System.Threading.Tasks;

namespace Candidate.Domain.Candidates
{
    public interface ICandidateDatabaseService
    {
        Task<List<CandidateDto>> RetrieveAsync(List<string> skills);
        Task StoreAsync(CandidateDto candidate);
    }
}