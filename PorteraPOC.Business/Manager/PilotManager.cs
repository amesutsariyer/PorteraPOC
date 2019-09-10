using PorteraPOC.Business.Service;
using PorteraPOC.DataAccess;
using PorteraPOC.DataAccess.Interface;
using PorteraPOC.Dto;
using PorteraPOC.Entity;

namespace PorteraPOC.Business.Manager
{
    public class PilotManager : IPilotService
    {

        private readonly IGenericRepository<Pilot> _pilotRepository;
        private readonly IUnitOfWork _uow;
        public PilotManager(IUnitOfWork uow)
        {
            _uow = uow;
            _pilotRepository = _uow.GetRepository<Pilot>();
        }
        public ServiceResult GetById(string id)
        {
            var pilotEntity = _pilotRepository.GetById(id);
            var data = AutoMapper.Mapper.Map<PilotDto>(pilotEntity);
            if (pilotEntity != null)
            {
                return Result.ReturnAsSuccess(data: data);
            }
            return Result.ReturnAsResultNotFound(data: data);
        }
    }
}
