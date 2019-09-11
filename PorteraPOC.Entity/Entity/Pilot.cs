using System;

namespace PorteraPOC.Entity
{
    public class Pilot :IKey<string>
    {
        public string Id { get; set; }
        public string SerialNumber { get; set; }
    }
}

