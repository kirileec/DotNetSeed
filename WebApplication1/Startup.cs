using ApiBase;
using ApiBase.Attributes;
using EFCore;
using Hei.Captcha;
using Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var ImageDir = Configuration.GetSection("StaticConfig").GetValue<string>("ImageDir");
            Directory.CreateDirectory(ImageDir);
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //日志
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //.WriteTo.EventLog("JDManagerNew")
                .WriteTo.RollingFile("./logs/log-{Date}.log")
                .CreateLogger();
            var dsn = "DataSource=sh-cdb-3ql7v8s2.sql.tencentcdb.com;port=63816;DataBase=deploy;uid=qjroot;pwd=qijin=mysql;Character Set=utf8;";
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseMySql(dsn, ServerVersion.AutoDetect(dsn));
                options.LogTo(Log.Logger.Information, LogLevel.Information, null);
            }, ServiceLifetime.Transient);

            //缓存
            services.AddEasyCaching(options =>
            {
                options.UseInMemory("default");
                //options.UseRedis(c =>
                //{
                //    c.DBConfig.KeyPrefix = "DotNetSeed_";
                //    c.DBConfig.Database = 2;
                //    c.DBConfig.Endpoints.Add(new ServerEndPoint("127.0.0.1", 6379));
                //},"redis").UseRedisLock() ;
            });

            services.AddControllers(opt =>
            {
                opt.Filters.Add<ExceptionFilter>();
            }).AddNewtonsoftJson(opt =>
            {
               
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).AddJsonOptions(options =>
            {
                //格式化日期时间格式
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //数据格式首字母小写
                //options.JsonSerializerOptions.PropertyNamingPolicy =JsonNamingPolicy.CamelCase;
                //数据格式原样输出
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //取消Unicode编码
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //忽略空值
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                //允许额外符号
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            //日志
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(dispose: true);
            });

            //这里不能使用OpenAPI3.0 因为3.0的要求, 当我们加入一个header叫Authorization的参数时, swagger ui会忽略这个参数, 必须使用右上角的Authorize按钮那种方式才可以传
            services.AddSwaggerDocument(doc =>
            {
                doc.Title = "DotNetSeed";
                doc.Description = "DotNetSeed";
                doc.UseControllerSummaryAsTagDescription = true;
                doc.UseXmlDocumentation = true;
                doc.GenerateEnumMappingDescription = true;
                doc.OperationProcessors.Add(new AuthAttribute());



            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //远程客户端IP获取
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            //cors 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", policy =>
                {

                    policy.AllowAnyMethod()
                   .SetIsOriginAllowed(_ => true)
                   .AllowAnyHeader()
                   .AllowCredentials();
                });

            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue; // or your desired value
            });
            services.AddHeiCaptcha(); //验证码服务
           
            //GlobalServiceProvider.ServiceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOpenApi();
            app.UseSwaggerUi3(o =>
            {
                o.AdditionalSettings.Add("filter", true);
                //o.AdditionalSettings["defaultModelsExpandDepth"]=-1;
                //o.AdditionalSettings["displayOperationId"] = true;
                o.AdditionalSettings["tryItOutEnabled"] = true;
                o.AdditionalSettings["layout"] = "BaseLayout";
                o.CustomJavaScriptPath = "/swagger_inject.js";
                o.CustomStylesheetPath = "/swagger_theme.css";

            });
            app.UseCors("AllowAnyOrigin");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/swagger_inject.js", (HttpContext context) =>
                {
                    return context.Response.SendFileAsync("swagger_inject.js");

                });
                endpoints.MapGet("/swagger_theme.css", (HttpContext context) =>
                {
                    return context.Response.SendFileAsync("swagger_theme.css");
                });
            });
            GlobalServiceProvider.ServiceProvider = app.ApplicationServices;

        }
    }
}
