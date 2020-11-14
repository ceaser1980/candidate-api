using System.Collections.Generic;
using System.Threading.Tasks;

namespace Candidate.Domain.Candidates
{
    public interface ICandidateService
    {
        Task<Candidate> RetrieveCandidatesWithSkills(List<string> skills);

        Task StoreCandidate(Candidate candidate);
    }
}