using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task Given_Retrieving_Candidate_With_Single_Matched_Skill()
        {
            //arrange
            const string skill = "testSkill";
            const string candidateName = "Mr Test";
            Guid candidateId = Guid.NewGuid();
            var skills = new List<string>
            {
                skill
            };
            var candidates = new List<CandidateDto>
            {
                new CandidateDto
                {
                    Id = candidateId,
                    Name = candidateName,
                    Skills = new List<SkillDto>
                    {
                        new SkillDto(skill)
                    }
                }
            };
            
            Mock.Get(_candidateDataService)
                .Setup(x => x.RetrieveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidates);

            //act
            var result = await _candidateService.RetrieveCandidatesWithSkills(skills, CancellationToken.None);

            //assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(candidateId, result.AsT0.Id);
            Assert.AreEqual(candidateName, result.AsT0.Name);
            Assert.AreEqual(skill, result.AsT0.Skills.FirstOrDefault());
        }

        [Test]
        public async Task Given_Retrieving_Candidate_With_Multiple_Skills_One_Match()
        {
            //arrange
            const string skill1 = "testSkill0";
            const string skill2 = "testSkill1";
            const string candidateName = "Mr Test";
            Guid candidateId = Guid.NewGuid();
            var skills = new List<string>
            {
                skill1,skill2
            };
            var candidates = new List<CandidateDto>
            {
                new CandidateDto
                {
                    Id = candidateId,
                    Name = candidateName,
                    Skills = new List<SkillDto>
                    {
                        new SkillDto(skill1)
                    }
                }
            };
            
            Mock.Get(_candidateDataService)
                .Setup(x => x.RetrieveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidates);
            
            //act
            var result = await _candidateService.RetrieveCandidatesWithSkills(skills, CancellationToken.None);

            //assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(candidateId, result.AsT0.Id);
            Assert.AreEqual(candidateName, result.AsT0.Name);
            Assert.AreEqual(1, result.AsT0.Skills.Length);
            Assert.AreEqual(skill1, result.AsT0.Skills.FirstOrDefault());
        }
        
        [Test]
        public async Task Given_Retrieving_Candidate_With_Multiple_Matched_Skills()
        {
            //arrange
            const string skill1 = "testSkill0";
            const string skill2 = "testSkill1";
            const string candidateName = "Mr Test";
            Guid candidateId = Guid.NewGuid();
            var skills = new List<string>
            {
                skill1,skill2
            };
            var candidates = new List<CandidateDto>
            {
                new CandidateDto
                {
                    Id = candidateId,
                    Name = candidateName,
                    Skills = new List<SkillDto>
                    {
                        new SkillDto(skill1),
                        new SkillDto(skill2)
                    }
                }
            };
            
            Mock.Get(_candidateDataService)
                .Setup(x => x.RetrieveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(candidates);
            
            //act
            var result = await _candidateService.RetrieveCandidatesWithSkills(skills, CancellationToken.None);

            //assert
            Assert.IsTrue(result.IsT0);
            Assert.AreEqual(candidateId, result.AsT0.Id);
            Assert.AreEqual(candidateName, result.AsT0.Name);
            Assert.AreEqual(2, result.AsT0.Skills.Length);

            for (var i = 0; i <= result.AsT0.Skills.Length - 1; i++)
            {
                Assert.AreEqual("testSkill" + i, result.AsT0.Skills[i]);
            }
        }
    }
}