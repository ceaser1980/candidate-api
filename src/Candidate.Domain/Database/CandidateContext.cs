using Candidate.Domain.Candidates;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Domain.Database
{
    /// <summary>
    /// Context for interacting with data store
    /// </summary>
    public class CandidateContext : DbContext
    {
        /// <summary>
        /// Constructor for interacting with data store
        /// </summary>
        /// <param name="options"></param>
        public CandidateContext(DbContextOptions<CandidateContext> options) :base (options)
        {
        }
        
        /// <summary>
        /// Dbset for candidates
        /// </summary>
        public DbSet<CandidateDto> Candidates { get; set; }
    }
}