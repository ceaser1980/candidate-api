using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Candidate.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Domain.Candidates
{
    ///<inheritdoc/>
    public class CandidateDataService : ICandidateDataService
    {
        private readonly CandidateContext _context;
        
        /// <summary>
        /// Constructor for candidate data service
        /// </summary>
        /// <param name="context">Candidate context for communicating with data store</param>
        public CandidateDataService(CandidateContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        ///<inheritdoc/>
        public async Task<List<CandidateDto>> RetrieveAsync(CancellationToken cancellationToken)
        {
            await using (_context)
            {
                return await _context.Candidates
                    .Include(x => x.Skills)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

        ///<inheritdoc/>
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