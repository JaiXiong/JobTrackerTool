using JobData.Dtos;
using JobData.Entities;
using AutoMapper;

namespace Utils.AutoMapper
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<JobProfile, JobProfileDto>();
            CreateMap<EmployerProfile, EmployerProfileDto>();
            CreateMap<EmployerProfile, EmployerProfilePagingDto>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<JobAction, JobActionDto>();
            CreateMap<Detail, DetailDto>();
        }
    }
}
