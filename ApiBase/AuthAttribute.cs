
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Linq;
using System.Reflection;

namespace ApiBase
{
    /// <summary>
    /// 需要token
    /// <para>可以用于Controller或者方法</para>
    /// <para>如果同时使用了 AllowAnonymousAttribute 则该接口无需token也可以访问</para>
    /// <para>不加也是匿名访问</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AuthAttribute : ActionFilterAttribute, IActionFilter, IOperationProcessor,IDocumentProcessor
    {
        /// <summary>
        ///
        /// </summary>
        public AuthAttribute()
        {
            
        }




       // public bool Visistor = false;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as BaseController;
            var httpContext = controller.HttpContext;

            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            string actionName = context.ActionDescriptor.RouteValues["action"];
            controller.GetLogger().LogInformation($"Controller:{controllerName} Action:{actionName} AuthAttribute OnActionExecuting");

            if (controllerName == "")
            {
                return;
            }
            else
            {
                var token = httpContext.Request.Headers["Authorization"];
                if (token.Count()<=0)
                {
                    var t = context.Controller.GetType();
                    if (t.GetMethod(actionName).GetCustomAttribute<AllowAnonymousAttribute>(true)!=null)
                    {
                        return;
                    }
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(new ResponseData<string>
                    {
                        code = BaseController.CODE_UNAUTH,
                        msg = "接口需要鉴权"
                    });
                    return;
                } 
                else
                {
                    //var u = Provider<CurrentUser>.ValidateToken(token);
                    //if (u==null)
                    //{
                    //    context.HttpContext.Response.StatusCode = 401;
                    //    context.Result = new JsonResult(new BaseResponse<string>
                    //    {
                    //        code = BaseController.CODE_UNAUTH,
                    //        msg = "鉴权失败, token无效或已过期"
                    //    });
                    //    return ;

                    //} 
                    //else
                    //{
                    //    if (Validate!=UserRoleEnum.Other)
                    //    {
                    //        switch (Validate)
                    //        {
                    //            case UserRoleEnum.All:
                    //                break;

                    //            case UserRoleEnum.Little:
                    //                if (u.UserType == SysUserType.Merchant)
                    //                {
                    //                    //ReturnMethodNotAllowed(actionContext);
                    //                    context.Result = new JsonResult(new BaseResponse<string>
                    //                    {
                    //                        code = BaseController.CODE_UNAUTH,
                    //                        msg = "无权访问"
                    //                    });
                    //                    // return;
                    //                    return;
                    //                }
                    //                break;
                    //            case UserRoleEnum.Back:
                    //                if (u.UserType != SysUserType.DeputyDirector && u.UserType != SysUserType.PoliceAdmin && u.UserType != SysUserType.Super)
                    //                {
                    //                    //ReturnMethodNotAllowed(actionContext);
                    //                    context.Result = new JsonResult(new BaseResponse<string>
                    //                    {
                    //                        code = BaseController.CODE_UNAUTH,
                    //                        msg = "无权访问"
                    //                    });
                    //                    return;
                    //                }
                    //                break;

                    //            default:
                    //                // actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
                    //                return;
                    //        }
                    //        if (!u.Roles.Contains(((int)Validate).ToString()))
                    //        {
                    //            context.HttpContext.Response.StatusCode = 403;
                    //            context.Result = new JsonResult(new BaseResponse<string>
                    //            {
                    //                code = BaseController.CODE_UNPERMISSION,
                    //                msg = "鉴权失败, 无权访问"
                    //            });
                    //            return;
                    //        }
                    //    }



                    //}

                    //if (Validate != UserRoleEnum.Other)
                    //{
                       

                    //}



                    //var identity = new Identity<CurrentUser>(u);
                    //var principal = new ClaimsPrincipal(identity);
                    //httpContext.User = principal;
                }


                //if (!(context.HttpContext.User.Identity is Identity))
                //{
                //    var result = new JsonResult(new OperateResponse(ResponseType.Unauthorized));
                //    context.Result = result;
                //    return;
                //}

                //var user = ((Identity)context.HttpContext.User.Identity).CurrentUser;
                ////非游客，判断token是否游客的token
                //if (!Visistor)
                //{
                //    if (user.IsVisitor != Visistor)
                //    {
                //        var result = new JsonResult(new OperateResponse(ResponseType.Unauthorized));
                //        context.Result = result;
                //        return;
                //    }
                //}

                //权限判断
                //if (Validate != UserRoleEnum.Other)
                //{
                //    List<UserRoleEnum> listRole = ((Identity)context.HttpContext.User.Identity).CurrentUser.UserRole;
                //    if (!listRole.Contains(Validate))
                //    {
                //        var result = new JsonResult(new OperateResponse(ResponseType.Unauthorized));
                //        context.Result = result;
                //        return;
                //    }

                //}
            }
           
        
        }

        

        private static void HandleActionAllowAnoymous(NSwag.OpenApiOperation operation, object[] actionAttributes)
        {
            var isRequired = true;
            if (actionAttributes.Any(t => t.GetType() == typeof(AllowAnonymousAttribute)))
            {
                isRequired = false;
            }

            operation.Parameters.Insert(0,new NSwag.OpenApiParameter
            {
                Name = "Authorization",
                Kind = NSwag.OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = isRequired,
                Schema= new JsonSchema { Type = NJsonSchema.JsonObjectType.String },
                Description = "[Token]",
                Default = "1",
            }) ;
            operation.Responses.Clear();
            operation.Responses.Add("200", new NSwag.OpenApiResponse
            {
                Schema = JsonSchema.FromType<ResponseData<string>>()

            }) ;


            if (isRequired)
            {
                operation.Responses.Add("401", new NSwag.OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new NSwag.OpenApiResponse { Description = "Forbidden" });
            }
        }

        public bool Process(OperationProcessorContext context)
        {
            var declareTypeAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true);
            var actionAttributes = context.MethodInfo.GetCustomAttributes(true);
            // 先判断Controller有没有[Auth]
            if (declareTypeAttributes.Any(t => t.GetType() == typeof(AuthAttribute)))
            {
                if (declareTypeAttributes.Any(t => t.GetType() == typeof(AllowAnonymousAttribute))) //判断Controller是否有[AllowAnonymous] 有则非必须
                {
                    //Controller有AllowAnonymous则判断Action是否有AllowAnonymous
                    HandleActionAllowAnoymous(context.OperationDescription.Operation, actionAttributes);
                    
                    return true;

                }
                else //Controller没有AllowAnonymous 则也 继续判断Action的
                {
                    HandleActionAllowAnoymous(context.OperationDescription.Operation, actionAttributes);
                    return true;
                }

            }
            else //Controller 没有Auth 判断Action是否有
            {
                if (actionAttributes.Any(t => t.GetType() == typeof(AuthAttribute)))
                {
                    HandleActionAllowAnoymous(context.OperationDescription.Operation, actionAttributes);
                    return true;
                }
            }


            return true;
        }

        public void Process(DocumentProcessorContext context)
        {
            
        }
    }

    


    
}
