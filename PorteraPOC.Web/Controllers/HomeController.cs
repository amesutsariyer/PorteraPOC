using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PorteraPOC.Business.Service;
using PorteraPOC.Dto;
using PorteraPOC.Dto.Validations;
using PorteraPOC.Entity;
using Serilog;
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
        /// <summary>
        /// This method generate query string with param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSerialWithId(string param)
        {
            var redirectUrl = Startup.PublicConfiguration.GetSection("PorteraSettings:Url").Value;
            try
            {
                var validateResult = ValidateParam(param);
                if (validateResult.IsValid)
                {
                    var response = GetPilotWithWatch(param);
                    if (response.ResultCode == HttpStatusCode.NotFound)
                    {
                        Serilog.Log.Error(response.Message);
                        return View("Failure", response.Message);
                    }
                    return Redirect(redirectUrl + $"/{(response.Data as PilotDto).SerialNoWithId}");
                }
                Serilog.Log.Error(validateResult.Errors.FirstOrDefault().ErrorMessage);
                return View("Failure", validateResult.Errors.FirstOrDefault().ErrorMessage);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex.Message);
                return View("Failure", ex.Message);
                throw ex;
            }
        }

        private ValidationResult ValidateParam(string param)
        {
            PilotDto dto = new PilotDto();
            PilotDtoValidation validator = new PilotDtoValidation();
            dto.Id = param ?? "";
            return validator.Validate(dto);
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
