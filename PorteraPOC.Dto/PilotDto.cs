﻿using System;

namespace PorteraPOC.Dto
{
    public class PilotDto
    {
        public PilotDto()
        {

        }
        public PilotDto(string id, string serialNumber)
        {
            SerialNoWithId = serialNumber+id;
            Id = id;
            SerialNumber = serialNumber;
        }

        public string Id { get; set; }
        public string SerialNumber { get; set; }
        public string SerialNoWithId { get; set; }

    }
}
