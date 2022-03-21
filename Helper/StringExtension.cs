using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helper
{
    public static class StringExtension
    {
        #region 空判断
        public static bool IsNullOrEmpty(this string inputStr)
        {
            return string.IsNullOrEmpty(inputStr);
        }

        public static bool IsNullOrWhiteSpace(this string inputStr)
        {
            return string.IsNullOrWhiteSpace(inputStr);
        }


        public static bool IsNotNullOrEmpty(this string _this)
        {
            return !string.IsNullOrEmpty(_this);
        }
        public static bool IsNotNullOrWhiteSpace(this string _this)
        {
            return !string.IsNullOrWhiteSpace(_this);
        }

        public static string Format(this string inputStr, params object[] obj)
        {
            return string.Format(inputStr, obj);
        }
        #endregion

        #region 常用正则表达式
        private static readonly Regex EmailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);

        private static readonly Regex MobileRegex = new Regex("^1[0-9]{10}$");

        private static readonly Regex PhoneRegex = new Regex(@"^(\d{3,4}-?)?\d{7,8}$");

        private static readonly Regex IpRegex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

        private static readonly Regex DateRegex = new Regex(@"(\d{4})-(\d{1,2})-(\d{1,2})");

        private static readonly Regex NumericRegex = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");

        private static readonly Regex ZipcodeRegex = new Regex(@"^\d{6}$");

        private static readonly Regex IdRegex = new Regex(@"^[1-9]\d{16}[\dXx]$");

        /// <summary>
        /// 是否中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(@"^[\u4e00-\u9fa5]+$", str);
        }

        /// <summary>
        /// 是否为邮箱名
        /// </summary>
        public static bool IsEmail(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return EmailRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否为手机号
        /// </summary>
        public static bool IsMobile(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return MobileRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否为固话号
        /// </summary>
        public static bool IsPhone(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return PhoneRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否为IP
        /// </summary>
        public static bool IsIp(this string s)
        {
            return IpRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否是身份证号
        /// </summary>
        public static bool IsIdCard(this string idCard)
        {
            if (string.IsNullOrEmpty(idCard))
                return false;
            return IdRegex.IsMatch(idCard);
        }

        /// <summary>
        /// 是否为日期
        /// </summary>
        public static bool IsDate(this string s)
        {
            return DateRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumeric(this string numericStr)
        {
            return NumericRegex.IsMatch(numericStr);
        }

        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        public static bool IsZipCode(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            return ZipcodeRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否是图片文件名
        /// </summary>
        /// <returns> </returns>
        public static bool IsImgFileName(this string fileName)
        {
            if (fileName.IndexOf(".", StringComparison.Ordinal) == -1)
                return false;

            string tempFileName = fileName.Trim().ToLower();
            string extension = tempFileName.Substring(tempFileName.LastIndexOf(".", StringComparison.Ordinal));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }

        #endregion

        #region 截取字符串SubString
        /// <summary>
        /// 截取分隔符前的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator">分隔符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetPreSubString(this string value, string separator, string defaultValue = "")
        {
            if (value == null)
            {
                return defaultValue;
            }

            return value.IndexOf(separator) < 0 ? defaultValue : value.Substring(0, value.IndexOf(separator));
        }

        /// <summary>
        /// 截取分隔符后的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator">分隔符</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetLastSubString(this string value, string separator, string defaultValue = "")
        {
            if (value == null)
            {
                return defaultValue;
            }

            var startIndex = value.LastIndexOf(separator) + separator.Length;
            var length = value.Length - startIndex;

            return value.LastIndexOf(separator) < 0 ? defaultValue : value.Substring(startIndex, length);
        }

        /// <summary>
        /// 截取分隔符后的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator">分隔符</param>
        /// <param name="length">长度</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetLastSubString(this string value, string separator, int length, string defaultValue = "")
        {
            if (value == null)
            {
                return defaultValue;
            }

            var startIndex = value.LastIndexOf(separator) + separator.Length;
            length = value.Length - startIndex >= length ? length : value.Length - startIndex;

            return value.LastIndexOf(separator) < 0 ? defaultValue : value.Substring(startIndex, length);
        }
        #endregion

        #region 字符格式化
        /// <summary>
        /// 字符格式化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Fmt(this string input, params object[] param)
        {
            if (input.IsNullOrWhiteSpace())
                return null;

            var result = string.Format(input, param);
            return result;
        }

        #endregion

        #region 格式化文本
        /// <summary>
        /// 格式化电话
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string FmtMobile(this string mobile)
        {
            if (!mobile.IsNullOrEmpty() && mobile.Length > 7)
            {
                var regx = new Regex(@"(?<=\d{3}).+(?=\d{4})", RegexOptions.IgnoreCase);
                mobile = regx.Replace(mobile, "****");
            }

            return mobile;
        }

        /// <summary>
        /// 格式化证件号码
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string FmtIdCard(this string idCard)
        {
            if (!idCard.IsNullOrEmpty() && idCard.Length > 10)
            {
                var regx = new Regex(@"(?<=\w{6}).+(?=\w{4})", RegexOptions.IgnoreCase);
                idCard = regx.Replace(idCard, "********");
            }

            return idCard;
        }

        /// <summary>
        /// 格式化银行卡号
        /// </summary>
        /// <param name="bankCark"></param>
        /// <returns></returns>
        public static string FmtBankCard(this string bankCark)
        {
            if (!bankCark.IsNullOrEmpty() && bankCark.Length > 4)
            {
                var regx = new Regex(@"(?<=\d{4})\d+(?=\d{4})", RegexOptions.IgnoreCase);
                bankCark = regx.Replace(bankCark, " **** **** ");
            }

            return bankCark;
        }

        #endregion

    }
}
