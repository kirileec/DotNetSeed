using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;



namespace ApiBase
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController: ControllerBase
    {
        private readonly ILogger _logger;
        [NonAction]
        public ILogger GetLogger()
        {
            return _logger;
        }
        public BaseController()
        {
            _logger = GlobalServiceProvider.ServiceProvider.GetRequiredService<ILogger<BaseController>>(); ;
        }

        public const string CODE_SUCCESS = "SUCCESS";
        public const string CODE_FAIL = "FAIL";
        public const string CODE_UNAUTH = "401";
        public const string CODE_UNPERMISSION = "403";
        public const string CODE_ERROR = "ERROR";
        private JsonResult Json(object data)
        {
            return new JsonResult(data);
        }
        protected JsonResult Success()
        {
            return Json(new ResponseData<object>
            {
                code = CODE_SUCCESS
            });
        }
        protected JsonResult Success(object data)
        {
            return Json(new ResponseData<object>
            {
                code = CODE_SUCCESS,
                data = data
            });
        }
        protected JsonResult Success<T>(T data)
        {
            return Json(new ResponseData<T>
            {
                code = CODE_SUCCESS,
                data = data
            });
        }
        protected JsonResult Data<T>(T data)
        {
            return Success(data);
        }
        protected JsonResult Data(object data)
        {
            return Success(data);
        }
        protected JsonResult Data<T>(IEnumerable<T> data)
        {
            return Success(data);
        }
        protected JsonResult Data<T>(long count,IEnumerable<T> data)
        {
            return Json(new ResponseData<IEnumerable<T>>
            {
                code = CODE_SUCCESS,
                count = count,
                data = data
            });
        }
        protected JsonResult Data<T>(int count, IEnumerable<T> data)
        {
            return Json(new ResponseData<IEnumerable<T>>
            {
                code = CODE_SUCCESS,
                count = count,
                data = data
            });
        }
        protected JsonResult Fail(string msg)
        {
            return Json(new ResponseData<string>
            {
                code = CODE_FAIL,
                msg = msg
            });
        }
        protected JsonResult Error(string msg)
        {
            return Json(new ResponseData<string>
            {
                code = CODE_ERROR,
                msg = msg
            });
        }
        protected JsonResult Error(Exception ex)
        {
            return Json(new ResponseData<string>
            {
                code = CODE_ERROR,
                msg = ex.Message,
                data = ex.StackTrace
            });
        }
        protected JsonResult Message(string msg)
        {
            return Json(new ResponseData<string>
            {
                code = CODE_SUCCESS,
                msg = msg
            });
        }

        private static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileLineNumber();
        }
        private static string GetFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(0).GetFileName();
        }
        protected string GetStack()
        {
            return $"{GetFileName()}:{GetLineNum()} - ";
        }
        protected string GetClientIP()
        {
            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];
            return remoteIpAddress;
        }


    }
}
