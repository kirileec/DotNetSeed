using EasyCaching.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBase
{
    public class TokenProvider<T>
    {
        private readonly TimeSpan TokenExpire = TimeSpan.FromSeconds(86400);
        private readonly IEasyCachingProvider _provider;
        public TokenProvider()
        {
            _provider = Global.GlobalServiceProvider.ServiceProvider.GetRequiredService<IEasyCachingProvider>();
        }
        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="qjUser"></param>
        /// <returns></returns>
        public string CreateToken(T user)
        {
            var token = Guid.NewGuid().ToString("N");
            _provider.Set("", user, TokenExpire);
            return token;
        }
        public T GetTokenInfo(string tokenKey)
        {
            var value = _provider.Get<T>(tokenKey);
            return value.HasValue ? value.Value : default(T);
        }
    }
}
