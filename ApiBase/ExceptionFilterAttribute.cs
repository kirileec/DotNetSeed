using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace ApiBase.Attributes
{
    /// <summary>
    /// 全局异常捕获并返回特定json
    /// </summary>
    public class ExceptionFilter: ExceptionFilterAttribute
    {
        private ILogger<ExceptionFilter> _logger;
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            this._logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var st = new StackTrace(context.Exception, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);

                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                var str = $"{ frame.GetFileName() }:{line}->{frame.GetMethod().Name}";


                context.Result = new JsonResult(new ResponseData<string>
                {
                    code = BaseController.CODE_ERROR,
                    msg = context.Exception.Message,
                    data = str
                });
                
                _logger.LogError(context.Exception.Message);
                context.ExceptionHandled = true; //表示错误已经处理过
            } else
            {
                base.OnException(context);
            }
            

            
        }


    }
}
