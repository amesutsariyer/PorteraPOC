using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PorteraPOC.Business.Service;
using PorteraPOC.Dto;
using PorteraPOC.Web.IDS.Models;

namespace PorteraPOC.Web.IDS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPilotService _pilotService;
        public HomeController(IPilotService pilotService)
        {
            _pilotService = pilotService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RedirectToIds(string param)
        {
            var redirectUrl = Startup.PublicConfiguration.GetSection("PorteraSettings:Url").Value;
            var response = _pilotService.GetById(param);
            if(response.ResultCode == System.Net.HttpStatusCode.NoContent)
            {
                return Redirect(redirectUrl + $"/{param}");
            }
            var id = (response.Data as PilotDto).SerialNoWithId;
            return Redirect(redirectUrl + $"/{id}");
        }
    }
}
