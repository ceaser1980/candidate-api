using Candidate.Domain.Candidates;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Domain.Database
{
    public class CandidateContext : DbContext
    {
        public CandidateContext(DbContextOptions<CandidateContext> options) :base (options)
        {
        }
        
        public DbSet<CandidateDto> Candidates { get; set; }
    }
}