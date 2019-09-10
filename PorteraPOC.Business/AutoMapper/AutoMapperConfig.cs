using AutoMapper;
using PorteraPOC.Dto;
using PorteraPOC.Entity;

namespace PorteraPOC.Business
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<PilotDto, Pilot>().ReverseMap().ForMember(x => x.SerialNoWithId, opt => opt.Ignore());
        }
    }
}
