using PensionManagement.Domain.Entities;

namespace PensionManagement.Application.Contracts
{
    public interface IContributionRepository
    {
        Task<Contribution> GetByIdAsync(Guid id);
        Task<IEnumerable<Contribution>> GetAllByMemberIdAsync(Guid memberId);
        Task<IEnumerable<Contribution>> GetAllPendingContributionsAsync();
        Task AddAsync(Contribution contribution);
        Task UpdateAsync(Contribution contribution);
    }
}
