using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Candidate.Domain.Candidates;
using Moq;
using NUnit.Framework;

namespace Candidate.Tests
{
    public class CandidateServiceTests
    {
        private ICandidateService _candidateService;
        private ICandidateDataService _candidateDataService;

        [SetUp]
        public void Setup()
        {
            _candidateDataService = Mock.Of<ICandidateDataService>();
            _candidateService = new CandidateService(_candidateDataService);
        }

        [Test]
        public async Task When_RetrieveCandidatesWithSkills_WithSingleSkill()
        {
            //arrange
            const string skill = "testString";
            var skills = new List<string>
            {
                skill
            };
            var candidateDtos = new List<CandidateDto>
            {
                new CandidateDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Mr Test",
                    Skills = new List<SkillDto>
                    {
                        new SkillDto(skill)
                    }
                }
            };
            
            Mock.Get(_candidateDataService)
                .Setup(x => x.RetrieveAsync(It.IsAny<List<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidateDtos);

            //act
            var result = await _candidateService.RetrieveCandidatesWithSkills(skills, CancellationToken.None);

            //assert
            Assert.IsTrue(result.IsT0);
            Assert.IsTrue(result.AsT0.Id != Guid.Empty);
        }
    }
}