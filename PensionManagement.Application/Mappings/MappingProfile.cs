using AutoMapper;
using PensionManagement.Domain.Entities;
using PensionManagement.Application.DTOs;

namespace PensionManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Member, MemberDto>().ReverseMap();
            
            // Mapping for Contribution
            CreateMap<Contribution, ContributionDto>().ReverseMap();

            // Mapping for Benefit
            CreateMap<Benefit, BenefitDto>().ReverseMap();
        }
    }
}
