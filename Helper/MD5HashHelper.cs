using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Helper
{
    public static class MD5HashHelper
    {
        /// <summary>
        /// md5 算32位小写哈希值
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string MD5Hex32(this string txt)
        {
            byte[] sor = Encoding.UTF8.GetBytes(txt);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                //加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
                strbul.Append(result[i].ToString("x2"));
            }
            return strbul.ToString();
        }
    }
}
