using AutoMapper;
using Chatison.Dtos;
using Chatison.Dtos.Contact;
using Chatison.Dtos.Group;
using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Contact;
using Chatison.ViewModels.Admin.Group;

namespace Chatison.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<SelectListItemDto, SelectListItemVm>();

            #region group mapping

            CreateMap<GroupListItemDto, GroupListItemVm>();

            CreateMap<PagedResultDto<GroupListItemDto>, JqDataTableResponseVm<GroupListItemVm>>()
                .ForMember(dest => dest.Draw, src => src.MapFrom(x => x.CurrentPage))
                .ForMember(dest => dest.RecordsTotal, src => src.MapFrom(x => x.TotalRecords))
                .ForMember(dest => dest.RecordsFiltered, src => src.MapFrom(x => x.TotalRecordsFiltered))
                .ForMember(dest => dest.Data, src => src.MapFrom(x => x.ResultSet));

            #endregion

            #region contact mapping

            CreateMap<ContactListItemDto, ContactListItemVm>();

            CreateMap<PagedResultDto<ContactListItemDto>, JqDataTableResponseVm<ContactListItemVm>>()
                .ForMember(dest => dest.Draw, src => src.MapFrom(x => x.CurrentPage))
                .ForMember(dest => dest.RecordsTotal, src => src.MapFrom(x => x.TotalRecords))
                .ForMember(dest => dest.RecordsFiltered, src => src.MapFrom(x => x.TotalRecordsFiltered))
                .ForMember(dest => dest.Data, src => src.MapFrom(x => x.ResultSet));

            CreateMap<ContactDetailDto, EditContactVm>();

            #endregion

        }
    }
}
