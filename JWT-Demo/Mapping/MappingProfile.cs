using AutoMapper;
using JWT_Demo.DTO;
using JWT_Demo.Models;

namespace JWT_Demo.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>()
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username)) 
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))      
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)) 
               .ForMember(dest => dest.UserId, opt => opt.Ignore())                       
               .ForMember(dest => dest.AccountNumber, opt => opt.Ignore())              
               .ForMember(dest => dest.Balance, opt => opt.Ignore());
        }
        
    }
}
