using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candidate.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Domain.Candidates
{
    public class CandidateDatabaseService : ICandidateDatabaseService
    {
        private readonly DbContextOptions<CandidateContext> _candidateDbContextOptions;
        
        public CandidateDatabaseService()
        {
            _candidateDbContextOptions = new DbContextOptionsBuilder<CandidateContext>()
                .UseInMemoryDatabase(databaseName: "Candidates")
                .Options;
        }

        public async Task<List<CandidateDto>> RetrieveAsync(List<string> skills)
        {
            await using (var context = new CandidateContext(_candidateDbContextOptions))
            {
                var candidates = await context.Candidates
                    .Include(x => x.Skills)
                    .ToListAsync();

                return candidates.Select(x => new
                    CandidateDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Skills = x.Skills
                            .Where(z => skills.Contains(z.Skill.ToLowerInvariant()))
                            .Where(z => z.Skill.Length > 0)
                            .ToList()
                    }).ToList();
            }
        }
        
        public async Task StoreAsync(CandidateDto candidate)
        {
            await using (var context = new CandidateContext(_candidateDbContextOptions))
            {
                await context.Candidates.AddAsync(candidate);
                await context.SaveChangesAsync();
            }
        }
    }
}