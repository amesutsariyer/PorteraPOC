using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
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
            return View("Ids");
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
            try
            {
                //must be 25 charachter
                if (param.Length == 25)
                {
                    bool flag = GetIdFromParam(param);
                    if (flag)
                    {
                        return Redirect("~/"+param);
                    }
                    return View("Failure", "Id number doesn't exist on database");
                }
                else
                {
                    var validateResult = ValidateParam(param);
                    Serilog.Log.Error(validateResult.Errors.FirstOrDefault().ErrorMessage);
                    return View("Failure", validateResult.Errors.FirstOrDefault().ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex.Message);
                return View("Failure", ex.Message);
                throw ex;
            }
        }

        private bool GetIdFromParam(string param)
        {
            var sn = param.Substring(0, 14);
            var id = param.Substring(14, 11);
            var response = _pilotService.GetById(id);
            var dto = (response.Data as PilotDto);
            if (dto != null && dto.SerialNumber == sn)
                return true;
            return false;
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
