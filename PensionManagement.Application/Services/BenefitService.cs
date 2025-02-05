using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Enums;

namespace PensionManagement.Application.Services
{
    public class BenefitService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IContributionRepository _contributionRepository;
        private readonly IBenefitRepository _benefitRepository;

        public BenefitService(IMemberRepository memberRepository, IContributionRepository contributionRepository, IBenefitRepository benefitRepository)
        {
            _memberRepository = memberRepository;
            _contributionRepository = contributionRepository;
            _benefitRepository = benefitRepository;
        }

        public async Task<bool> IsEligibleForBenefit(Guid memberId, BenefitType benefitType)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null) return false;

            var totalContributions = (await _contributionRepository.GetAllByMemberIdAsync(memberId))
                .Sum(c => c.Amount);

            // Calculate Age from DateOfBirth
            int age = DateTime.UtcNow.Year - member.DateOfBirth.Year;
            if (DateTime.UtcNow < member.DateOfBirth.AddYears(age)) age--;

            // Retirement: Check if member is 60+ years old and has enough contributions
            if (benefitType == BenefitType.Retirement && age >= 60 && totalContributions > 50000)
                return true;

            // Disability: Assume Self-Employed members are considered disabled for benefits
            if (benefitType == BenefitType.Disability && member.MemberType == MemberType.SelfEmployed)
                return true;

            // Survivor: Assume deleted members (IsDeleted = true) represent deceased members
            if (benefitType == BenefitType.Survivor && member.IsDeleted)
                return true;

            return false;
        }
        
        public async Task<decimal> CalculateBenefitAmount(Guid memberId, BenefitType benefitType)
        {
            var totalContributions = (await _contributionRepository.GetAllByMemberIdAsync(memberId))
                .Sum(c => c.Amount);

            if (benefitType == BenefitType.Retirement)
                return totalContributions * 1.2m; // 20% extra on retirement

            if (benefitType == BenefitType.Disability)
                return totalContributions * 1.5m; // 50% extra for disability

            if (benefitType == BenefitType.Survivor)
                return totalContributions; // Full amount for survivors

            return 0;
        }

        public async Task<IEnumerable<BenefitDto>> GetAllByMemberIdAsync(Guid memberId)
        {
            var benefits = await _benefitRepository.GetAllByMemberIdAsync(memberId);

            return benefits.Select(b => new BenefitDto
            {
                MemberId = b.MemberId,
                BenefitType = b.BenefitType,
                Amount = b.Amount
            }).ToList();
        }


    }
}
