using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Candidate.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Domain.Candidates
{
    public class CandidateDatabaseService : ICandidateDatabaseService
    {
        private readonly CandidateContext _context;
        
        public CandidateDatabaseService(CandidateContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<CandidateDto>> RetrieveAsync(List<string> skills)
        {
            await using (_context)
            {
                var candidates = await _context.Candidates
                    .Include(x => x.Skills)
                    .ToListAsync();

                return candidates
                    .Select(x => new
                        CandidateDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Skills = x.Skills
                                .Where(z => skills.Contains(z.Skill.ToLowerInvariant()))
                                .ToList()
                        })
                    .Where(item => item.Skills.Count > 0)
                    .ToList();
            }
        }

        public async Task StoreAsync(CandidateDto candidate)
        {
            await using (_context)
            {
                await _context.Candidates.AddAsync(candidate);
                await _context.SaveChangesAsync();
            }
        }
    }
}