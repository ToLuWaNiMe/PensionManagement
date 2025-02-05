using AutoMapper;
using PensionManagement.Domain.Entities;
using PensionManagement.Application.DTOs;

namespace PensionManagement.Application.Mappings
{
    public class MemberProfile : Profile
    {
        public MemberProfile() 
        {
            CreateMap<Member, MemberDto>();
        }
    }
}
