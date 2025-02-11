using PensionManagement.Domain.Entities;

namespace PensionManagement.Application.Contracts
{
    public interface IBenefitRepository
    {
        Task<Benefit> GetByIdAsync(int id);
        Task<IEnumerable<Benefit>> GetAllByMemberIdAsync(int memberId);
        Task AddAsync(Benefit benefit);
        Task UpdateAsync(Benefit benefit);
    }
}
