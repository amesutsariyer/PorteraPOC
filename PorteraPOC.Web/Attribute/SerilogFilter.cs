using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace PorteraPOC.Web.Attribute
{
    public class SerilogFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                Log.Error(context.Exception, context.Exception.Message);

            base.OnActionExecuted(context);
        }
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    Log.Error("test", "test");
        //    base.OnActionExecuting(context);
        //}

    }
}
