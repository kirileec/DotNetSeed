using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Helper
{
    public static class StringFormat
    {
        /// <summary>
        /// 将传入的字符串中间部分字符替换成特殊字符
        /// </summary>
        /// <param name="idcard">需要替换的字符串</param>
        /// <param name="startLen">前保留长度</param>
        /// <param name="endLen">尾保留长度</param>
        /// <param name="specialChar">特殊字符</param>
        /// <returns>被特殊字符替换的字符串</returns>
        public static string ToMaskIDCard(this string idcard,int startLen = 4, int endLen = 4, char specialChar = '*')
        {
            try
            {
                int lenth = idcard.Length - startLen - endLen;
                string replaceStr = idcard.Substring(startLen, lenth);
                string specialStr = string.Empty;
                for (int i = 0; i < replaceStr.Length; i++)
                {
                    specialStr += specialChar;
                }
                idcard = idcard.Replace(replaceStr, specialStr);
            }
            catch (Exception ex)
            {
                throw;
            }
            return idcard;
        }

        


    }
}
