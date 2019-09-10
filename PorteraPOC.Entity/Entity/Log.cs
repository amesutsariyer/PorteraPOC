using System;
using System.Collections.Generic;
using System.Text;

namespace PorteraPOC.Entity  
{
    public class Log : IKey<Guid>
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public string ResponseTimeFormatted { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Parameters { get; set; }
        public string Response { get; set; }
    }
}
