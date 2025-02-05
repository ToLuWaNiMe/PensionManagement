using Hangfire;
using PensionManagement.Application.Contracts;

namespace PensionManagement.Application.Services
{
    public class BackgroundJobService
    {
        private readonly IContributionRepository _contributionRepository;

        public BackgroundJobService(IContributionRepository contributionRepository)
        {
            _contributionRepository = contributionRepository;
        }

        // Background job to validate contributions
        [AutomaticRetry(Attempts = 3)]
        public async Task ValidateContributions()
        {
            var contributions = await _contributionRepository.GetAllPendingContributionsAsync();

            foreach (var contribution in contributions)
            {
                contribution.IsValid = contribution.Amount >= 100;
                await _contributionRepository.UpdateAsync(contribution);
            }
        }

    }
}
