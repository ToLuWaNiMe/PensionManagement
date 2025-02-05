using AutoMapper;
using PensionManagement.Application.DTOs;
using PensionManagement.Domain.Entities;
using PensionManagement.Application.Contracts;

namespace PensionManagement.Application.Services
{
    public class ContributionService
    {
        private readonly IContributionRepository _repository;
        private readonly IMapper _mapper;

        public ContributionService(IContributionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContributionDto>> GetAllByMemberIdAsync(Guid memberId)
        {
            var contributions = await _repository.GetAllByMemberIdAsync(memberId);
            return _mapper.Map<IEnumerable<ContributionDto>>(contributions);
        }

        public async Task AddAsync(ContributionDto contributionDto)
        {
            var contribution = _mapper.Map<Contribution>(contributionDto);
            await _repository.AddAsync(contribution);
        }

        public async Task UpdateAsync(Guid id, ContributionDto contributionDto)
        {
            var existingContribution = await _repository.GetByIdAsync(id);
            if (existingContribution != null)
            {
                _mapper.Map(contributionDto, existingContribution);
                await _repository.UpdateAsync(existingContribution);
            }
        }
    }
}
