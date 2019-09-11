using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PorteraPOC.Business.Service;
using PorteraPOC.Dto;
using PorteraPOC.Dto.Validations;
using System;
using System.Diagnostics;
using System.Linq;
namespace PorteraPOC.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPilotService _pilotService;
        private readonly IHttpContextAccessor _httpContext;
        public HomeController(IPilotService pilotService, IHttpContextAccessor httpContext)
        {
            _pilotService = pilotService;
            _httpContext = httpContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Ids(string res)
        {
            if (res.Length == 25)
            {
                bool flag = GetIdFromParam(res);
                if (flag)
                {
                    return View("Ids");
                }
                return View("Failure", "Id number doesn't exist on database");
            }
            else
            {
                return View("Failure", "Invalid Request");
            }
        }

        public IActionResult Failure()
        {
            return View();
        }

        private bool GetIdFromParam(string param)
        {
            var sn = param.Substring(0, 14);
            var id = param.Substring(14, 11);
            var response = GetPilotWithWatch(id);
            var dto = (response.Data as PilotDto);
            if (dto != null && dto.SerialNumber == sn)
                return true;
            return false;
        }

        public Business.ServiceResult GetPilotWithWatch(string param)
        {
            var watch = Stopwatch.StartNew();

            var response = _pilotService.GetById(param);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Serilog.Log.Information($"Parameter {param}.Serial number Found on database {elapsedMs}ms");
            return response;
        }
    }
}
