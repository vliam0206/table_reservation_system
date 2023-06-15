using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Models.TableModels;

namespace WebAPI.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Table, TableModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<TableModel, Table>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TableEnum>(src.Status)));

        CreateMap<Table, TableViewModel>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<TableViewModel, Table>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<TableEnum>(src.Status)));
    }
}
