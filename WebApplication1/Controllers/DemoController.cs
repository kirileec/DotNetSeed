using ApiBase;
using EasyCaching.Core;
using EFCore;
using Hei.Captcha;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace WebApplication1.Controllers
{
    
    public class DemoController : BaseController
    {
        
        //[HttpGet("GetCaptcha")]
        public JsonResult GetCaptcha([FromServices]SecurityCodeHelper _securityCode, [FromServices] IEasyCachingProviderFactory _factory)
        {
            var code = _securityCode.GetRandomEnDigitalText(4);
            var imgbyte = _securityCode.GetGifEnDigitalCodeByte(code);
            var id = _securityCode.GetRandomEnDigitalText(26);
            var resp = new 
            {
                idKey = id,
                base64string = "data:image/gif;base64," + Convert.ToBase64String(imgbyte)
            };
            _factory.GetCachingProvider("default").Set(id, code, TimeSpan.FromSeconds(120));
            return Data(resp);
        }
        [Auth]
        //[HttpGet("NeedAuthorizationHeader")]
        public IActionResult NeedAuthorizationHeader(string hello)
        {
            return Message($"Hello2 {hello}!");
        }
        //[HttpGet("TestServiceProvider")]
        public JsonResult TestServiceProvider([FromServices] MyDbContext db)
        {
            var _db = GlobalServiceProvider.ServiceProvider.GetRequiredService<MyDbContext>();
            return Data(db.deploy_history.Where(e=>true).Count());
        }

    }
    
}
