using System;

namespace Candidate.Api
{
    /// <summary>
    /// Class representing a job candidate
    /// </summary>
    public class Candidate
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
        public string[] Skills { get; set; }
    }
}