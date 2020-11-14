using System;

namespace Candidate.Domain.Candidates
{
    /// <summary>
    /// Dto for database entry storing the skills
    /// </summary>
    public class SkillDto
    {
        /// <summary>
        /// constructor for storing a skill and assigning an Id
        /// </summary>
        /// <param name="skill">Skill to store</param>
        public SkillDto(string skill)
        {
            Id = Guid.NewGuid();
            Skill = skill;
        }
        
        /// <summary>
        /// Auto generated id for the skill
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Skill being stored
        /// </summary>
        public string Skill { get; set; }
    }
}