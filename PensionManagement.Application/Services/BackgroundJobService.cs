using Hangfire;
using PensionManagement.Application.Contracts;

namespace PensionManagement.Application.Services
{
    /// <summary>
    /// Service responsible for handling background jobs related to contributions.
    /// </summary>
    public class BackgroundJobService
    {
        private readonly IContributionRepository _contributionRepository;

        /// <summary>
        /// Initializes a new instance of <see cref="BackgroundJobService"/>.
        /// </summary>
        /// <param name="contributionRepository">Repository for managing contributions.</param>
        public BackgroundJobService(IContributionRepository contributionRepository)
        {
            _contributionRepository = contributionRepository;
        }

        /// <summary>
        /// Background job to validate contributions.
        /// Ensures that each contribution meets the minimum amount requirement.
        /// Retries up to 3 times in case of failure.
        /// </summary>
        [AutomaticRetry(Attempts = 3)]
        public async Task ValidateContributions()
        {
            var contributions = await _contributionRepository.GetAllPendingContributionsAsync();

            foreach (var contribution in contributions)
            {
                // Mark contribution as valid if it meets the minimum amount requirement.
                contribution.IsValid = contribution.Amount >= 100;
                await _contributionRepository.UpdateAsync(contribution);
            }
        }
    }
}
