using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using WebAPI.Models;
using WebAPI.Models.AccountModels;
using WebAPI.Models.ReservationModels;
using WebAPI.Models.TableModels;

namespace WebAPI.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {        
        CreateMap<Table, TableModel>().ReverseMap();
        CreateMap<Table, TableViewModel>().ReverseMap();

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

        CreateMap<Account, AccountViewModel>().ReverseMap();        
        CreateMap<Account, AccountModel>().ReverseMap();        
    }
}
