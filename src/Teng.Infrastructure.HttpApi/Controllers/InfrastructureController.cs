using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Teng.Infrastructure.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Teng.Infrastructure.Controllers
{
    /* Inherit your controllers from this class.
     */

    public abstract class InfrastructureController : AbpController
    {
        protected InfrastructureController()
        {
            LocalizationResource = typeof(InfrastructureResource);
        }

        protected SuccessResult<T> Ok<T>(T data, HttpStatusCode code = HttpStatusCode.OK, string msg = "")
        {
            return new SuccessResult<T>(data, code, msg);
        }
    }

    public class SuccessResult<T> : OkObjectResult
    {
        public SuccessResult(T data, HttpStatusCode code = HttpStatusCode.OK, string msg = "") : base(data)
        {
            Code = code;
            Data = data;
            Msg = msg;
        }

        public HttpStatusCode Code { get; set; }

        public T Data { get; set; }

        public string Msg { get; set; }
    }
}