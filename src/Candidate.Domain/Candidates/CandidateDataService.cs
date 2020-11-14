using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Candidate.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Domain.Candidates
{
    public class CandidateDataService : ICandidateDataService
    {
        private readonly CandidateContext _context;
        
        public CandidateDataService(CandidateContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<CandidateDto>> RetrieveAsync(CancellationToken cancellationToken)
        {
            await using (_context)
            {
                return await _context.Candidates
                    .Include(x => x.Skills)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        public async Task StoreAsync(CandidateDto candidate, CancellationToken cancellationToken)
        {
            await using (_context)
            {
                await _context.Candidates.AddAsync(candidate, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}