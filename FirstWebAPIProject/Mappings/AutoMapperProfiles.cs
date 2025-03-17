using AutoMapper;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Model.DTO;

namespace FirstWebAPIProject.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Region, RegionDTO>().ReverseMap();

            CreateMap<AddRegionDTO, Region>().ReverseMap();

            CreateMap<UpdateRegionDTO, Region>().ReverseMap();

            CreateMap<AddWalkDTO, Walk>().ReverseMap();

            CreateMap<Walk, WalkDTO>().ReverseMap();

            CreateMap<Difficulty, DifficultyDTO>().ReverseMap();

            CreateMap<UpdateWalkDTO, Walk>().ReverseMap();

        }
    }
}
