using System;

namespace Candidate.Domain.Candidates
{
    public class SkillDto
    {
        public SkillDto(string skill)
        {
            Id = Guid.NewGuid();
            Skill = skill;
        }
        
        public Guid Id { get; set; }
        
        public string Skill { get; set; }
    }
}