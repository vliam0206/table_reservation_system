using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Models.ReservationModels;
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

        CreateMap<Reservation, ReservationModel>()
            .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.CustomerInfo.FullName))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.CustomerInfo.Email))
            .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(src => src.CustomerInfo.PhoneNumber))
            .ReverseMap();
        CreateMap<Reservation, ReservationViewModel>()
            .ForMember(dest => dest.CustomerFullName, opt => opt.MapFrom(src => src.CustomerInfo.FullName))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.CustomerInfo.Email))
            .ForMember(dest => dest.CustomerPhoneNumber, opt => opt.MapFrom(src => src.CustomerInfo.PhoneNumber))
            .ReverseMap();
    }
}
