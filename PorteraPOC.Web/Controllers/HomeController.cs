using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PorteraPOC.Business.Service;
using PorteraPOC.Dto;
using PorteraPOC.Dto.Validations;
using PorteraPOC.Entity;

namespace PorteraPOC.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPilotService _pilotService;
        public HomeController(IPilotService pilotService)
        {
            _pilotService = pilotService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ValidRequest(string param)
        {
            return View("ValidRequest", param);
        }
        public IActionResult Failure()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetSerialWithId(string param)
        {
            var redirectUrl = Startup.PublicConfiguration.GetSection("PorteraSettings:Url").Value;
            try
            {
                PilotDto dto = new PilotDto();
                PilotDtoValidation validator = new PilotDtoValidation();
                dto.Id = param ?? "";

                var validateResult = validator.Validate(dto);
                if (validateResult.IsValid)
                {
                    var response = _pilotService.GetById(param);
                    if (response.ResultCode == HttpStatusCode.NotFound)
                    {
                        return View("Failure", response.Message);
                    }
                    return Redirect(redirectUrl + $"?param={param = (response.Data as PilotDto).SerialNoWithId}");

                    //   return RedirectToAction("ValidRequest", new { param = (response.Data as PilotDto).SerialNoWithId });
                }

                return View("Failure", validateResult.Errors.FirstOrDefault().ErrorMessage);
            }
            catch (Exception ex)
            {
                return Redirect(redirectUrl);
                throw ex;
            }
        }
    }
}
