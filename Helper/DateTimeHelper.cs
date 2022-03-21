using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /// <summary>
    /// 日期时间辅助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 日期转为unix时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long Unix(this DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long NowUnix()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        public static long NowUnixMs()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }
        public static string NowUnixMsString()
        {
            return NowUnixMs().ToString();
        }

        public static string NowUnixString()
        {
            return NowUnix().ToString();
        }
        public static long UnixMs(this DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }
        public static string UnixMsString(this DateTime dt)
        {
            return dt.UnixMs().ToString();
        }
    }
}
