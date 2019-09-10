using System;

namespace PorteraPOC.Dto
{
    public class PilotDto
    {
        public PilotDto()
        {

        }
        public PilotDto(string id, Guid serialNumber)
        {
            SerialNoWithId = id + serialNumber;
        }
 
        public string Id { get; set; }
        public Guid SerialNo { get; set; }
        public string SerialNoWithId { get; set; }

    }
}
