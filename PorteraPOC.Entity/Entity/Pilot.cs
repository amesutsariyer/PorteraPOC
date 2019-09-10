using System;

namespace PorteraPOC.Entity
{
    public class Pilot : BaseEntity, IKey<string>
    {
        public string Id { get; set; }
        public Guid SerialNumber { get; set; }
    }
}

