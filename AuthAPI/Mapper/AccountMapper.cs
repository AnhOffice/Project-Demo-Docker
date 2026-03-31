using AuthAPI.DTOs;
using AuthAPI.Model;
using AutoMapper;

namespace AuthAPI.Mapper
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            CreateMap<RegisterDTOs, Account>()
               .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Sẽ mã hóa password sau
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
