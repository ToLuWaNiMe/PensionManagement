using Hangfire;
using PensionManagement.Domain.Entities;
using PensionManagement.Domain.Enums;
using PensionManagement.Application.Contracts;

namespace PensionManagement.Application.Services
{
    /// <summary>
    /// Service responsible for processing benefit eligibility for members.
    /// Uses Hangfire for background job execution.
    /// </summary>
    public class BenefitProcessingService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IContributionRepository _contributionRepository;
        private readonly IBenefitRepository _benefitRepository;

        /// <summary>
        /// Initializes a new instance of <see cref="BenefitProcessingService"/>.
        /// </summary>
        /// <param name="memberRepository">Repository for accessing member data.</param>
        /// <param name="contributionRepository">Repository for accessing contributions.</param>
        /// <param name="benefitRepository">Repository for managing benefits.</param>
        public BenefitProcessingService(
            IMemberRepository memberRepository,
            IContributionRepository contributionRepository,
            IBenefitRepository benefitRepository)
        {
            _memberRepository = memberRepository;
            _contributionRepository = contributionRepository;
            _benefitRepository = benefitRepository;
        }

        /// <summary>
        /// Processes benefit eligibility for all members.
        /// Runs as a background job with automatic retry up to 3 times in case of failure.
        /// </summary>
        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessBenefitEligibility()
        {
            var members = await _memberRepository.GetAllAsync();

            foreach (var member in members)
            {
                // Calculate total contributions made by the member
                var totalContributions = (await _contributionRepository.GetAllByMemberIdAsync(member.Id))
                    .Sum(c => c.Amount);

                BenefitType? eligibleBenefit = null;

                // Calculate member's age based on Date of Birth
                int age = DateTime.UtcNow.Year - member.DateOfBirth.Year;
                if (DateTime.UtcNow < member.DateOfBirth.AddYears(age)) age--;

                // Determine benefit eligibility based on age, member type, and status
                if (age >= 60 && totalContributions > 50000)
                {
                    eligibleBenefit = BenefitType.Retirement;
                }
                else if (member.MemberType == MemberType.SelfEmployed)
                {
                    // Self-employed members may qualify for disability benefits
                    eligibleBenefit = BenefitType.Disability;
                }
                else if (member.IsDeleted)
                {
                    // Deleted members are considered deceased and qualify for survivor benefits
                    eligibleBenefit = BenefitType.Survivor;
                }

                // If the member is eligible for a benefit, create a benefit entry
                if (eligibleBenefit.HasValue)
                {
                    var benefit = new Benefit
                    {
                        MemberId = member.Id,
                        BenefitType = eligibleBenefit.Value,
                        Amount = totalContributions * 1.2m, // 20% bonus on total contributions
                        EligibilityStatus = EligibilityStatus.Eligible
                    };

                    await _benefitRepository.AddAsync(benefit);
                }
            }
        }
    }
}
