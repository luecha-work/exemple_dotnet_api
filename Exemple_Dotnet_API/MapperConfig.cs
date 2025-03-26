using AutoMapper;
using Entities;
using Shared.DTOs;

namespace Exemple_Dotnet_API
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Account, AccountDto>(); // Account -> AccountDto
            CreateMap<SingUpDto, Account>(); // SingUpDto -> Account
            // CreateMap<SingUpDto, Account>().ReverseMap(); // SingUpDto <-> Account
        }
    }
}
