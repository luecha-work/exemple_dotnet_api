using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Exemple_Dotnet_API;

namespace UnitTest.MockData
{
    public static class MapperMock
    {
        public static IMapper GetMapper()
        {
            var mappingProfile = new MapperConfig();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
            return new Mapper(configuration);
        }
    }
}