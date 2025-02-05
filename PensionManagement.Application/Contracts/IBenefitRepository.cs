using PensionManagement.Domain.Entities;

namespace PensionManagement.Application.Contracts
{
    public interface IBenefitRepository
    {
        Task<Benefit> GetByIdAsync(Guid id);
        Task<IEnumerable<Benefit>> GetAllByMemberIdAsync(Guid memberId);
        Task AddAsync(Benefit benefit);
        Task UpdateAsync(Benefit benefit);
    }
}
