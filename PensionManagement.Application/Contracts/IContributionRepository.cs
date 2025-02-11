using PensionManagement.Domain.Entities;

namespace PensionManagement.Application.Contracts
{
    public interface IContributionRepository
    {
        Task<Contribution> GetByIdAsync(int id);
        Task<IEnumerable<Contribution>> GetAllByMemberIdAsync(int memberId);
        Task<IEnumerable<Contribution>> GetAllPendingContributionsAsync();
        Task AddAsync(Contribution contribution);
        Task UpdateAsync(Contribution contribution);
    }
}
