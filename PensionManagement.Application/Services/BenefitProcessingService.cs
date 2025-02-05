using Hangfire;
using PensionManagement.Domain.Entities;
using PensionManagement.Domain.Enums;
using PensionManagement.Application.Contracts;

namespace PensionManagement.Application.Services
{
    public class BenefitProcessingService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IContributionRepository _contributionRepository;
        private readonly IBenefitRepository _benefitRepository;

        public BenefitProcessingService(
            IMemberRepository memberRepository,
            IContributionRepository contributionRepository,
            IBenefitRepository benefitRepository)
        {
            _memberRepository = memberRepository;
            _contributionRepository = contributionRepository;
            _benefitRepository = benefitRepository;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessBenefitEligibility()
        {
            var members = await _memberRepository.GetAllAsync();

            foreach (var member in members)
            {
                var totalContributions = (await _contributionRepository.GetAllByMemberIdAsync(member.Id))
                    .Sum(c => c.Amount);

                BenefitType? eligibleBenefit = null;

                //Calculate Age from DateOfBirth
                int age = DateTime.UtcNow.Year - member.DateOfBirth.Year;
                if (DateTime.UtcNow < member.DateOfBirth.AddYears(age)) age--;

                //Determine benefit eligibility based on actual properties
                if (age >= 60 && totalContributions > 50000)
                {
                    eligibleBenefit = BenefitType.Retirement;
                }
                else if (member.MemberType == MemberType.SelfEmployed)
                {
                    // Assuming Self-Employed members may qualify for disability benefits
                    eligibleBenefit = BenefitType.Disability;
                }
                else if (member.IsDeleted)
                {
                    // Assuming Deleted Members are considered "Deceased"
                    eligibleBenefit = BenefitType.Survivor;
                }

                if (eligibleBenefit.HasValue)
                {
                    var benefit = new Benefit
                    {
                        MemberId = member.Id,
                        BenefitType = eligibleBenefit.Value,
                        Amount = totalContributions * 1.2m, //20% bonus on total contributions
                        EligibilitySatus = true //Using correct property name instead of IsApproved
                    };

                    await _benefitRepository.AddAsync(benefit);
                }
            }
        }
    }
}
