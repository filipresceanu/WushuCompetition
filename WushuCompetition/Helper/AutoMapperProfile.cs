using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WushuCompetition.Dto;
using WushuCompetition.Dto.Identity;
using WushuCompetition.Models;

namespace WushuCompetition.Helper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Competition, CompetitionDto>();
            CreateMap<Participant, ParticipantDto>();
            CreateMap<ParticipantDto, Participant>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<AgeCategoryDto, AgeCategory>();
            CreateMap<AgeCategory,AgeCategoryDto>();
            CreateMap<RegisterDto, IdentityUser>();
            CreateMap<IdentityUser, RegisterDto>();

        }
    }
}
