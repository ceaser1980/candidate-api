using System;
using System.Collections.Generic;

namespace Candidate.Domain.Candidates
{
    /// <summary>
    /// Candidate Dto for database entry
    /// </summary>
    public class CandidateDto
    {
        /// <summary>
        /// unique id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// candidate skills
        /// </summary>
        public List<SkillDto> Skills { get; set; }
    }
}