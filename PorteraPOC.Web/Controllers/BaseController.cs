using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PorteraPOC.Web.Attribute;

namespace PorteraPOC.Web.Controllers
{
    [SerilogFilter]
    public class BaseController : Controller
    {
      
    }
}