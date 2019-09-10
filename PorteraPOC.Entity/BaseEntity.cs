using System;

namespace PorteraPOC.Entity
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }


    }
}

