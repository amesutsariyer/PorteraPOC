using System.Net;

namespace PorteraPOC.Business
{
    public class ServiceResult
    {
        public ServiceResult(string message = null, HttpStatusCode? resultCode = null, object data = null)
        {
            ResultCode = resultCode ?? HttpStatusCode.SeeOther;
            Data = data;
            Message = string.IsNullOrEmpty(message) ? "Transaction Failed" : message;
        }
        public HttpStatusCode ResultCode { get; }
        public string Message { get; }
        public object Data { get; }
    }
    public static class Result
    {
        public static ServiceResult ReturnAsSuccess(string message = null, object data = null)
        {
            return new ServiceResult(message ?? "Transaction Success", HttpStatusCode.OK, data);
        }
        public static ServiceResult ReturnAsFail(string message = null, object data = null)
        {
            return new ServiceResult(message ?? "Transaction Failed", HttpStatusCode.SeeOther, data);
        }
        public static ServiceResult ReturnAsResultNotFound(string message = null, object data = null)
        {
            return new ServiceResult(message ?? "Data Can not Found on Database.", HttpStatusCode.NoContent, data);
        }
    }
}