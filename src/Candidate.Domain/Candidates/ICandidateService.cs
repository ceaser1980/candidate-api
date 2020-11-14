using System.Collections.Generic;
using System.Threading.Tasks;
using OneOf.Types;
using OneOf;

namespace Candidate.Domain.Candidates
{
    public interface ICandidateService
    {
        Task<OneOf<Candidate, NotFound>> RetrieveCandidatesWithSkills(List<string> skills);

        Task StoreCandidate(Candidate candidate);
    }
}