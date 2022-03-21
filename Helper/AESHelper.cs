using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class AESCBCHelper
    {

        private const string   KEY = "qijin.cooc.nijiq";
        /// <summary>
        /// aes加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string AESEncrypt(this string content)
        {

            byte[] byteKEY = Encoding.UTF8.GetBytes(KEY);
            byte[] byteIV = Encoding.UTF8.GetBytes(KEY);

            byte[] byteContnet = Encoding.UTF8.GetBytes(content);

            var _aes = new RijndaelManaged();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;
            //_aes.BlockSize = 16;

            _aes.Key = byteKEY;
            _aes.IV = byteIV;

            var _crypto = _aes.CreateEncryptor(byteKEY, byteIV);
            byte[] decrypted = _crypto.TransformFinalBlock(
                byteContnet, 0, byteContnet.Length);

            _crypto.Dispose();

            return Convert.ToBase64String(decrypted);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptStr">要解密的串</param>
        /// <returns></returns>
        public static string AESDecrypt(this string decryptStr)
        {

            byte[] byteKEY = Encoding.UTF8.GetBytes(KEY);
            byte[] byteIV = Encoding.UTF8.GetBytes(KEY);

            byte[] byteDecrypt = System.Convert.FromBase64String(decryptStr);

            var _aes = new RijndaelManaged();
            _aes.Padding = PaddingMode.PKCS7;
            _aes.Mode = CipherMode.CBC;

            _aes.Key = byteKEY;
            _aes.IV = byteIV;

            var _crypto = _aes.CreateDecryptor(byteKEY, byteIV);
            byte[] decrypted = _crypto.TransformFinalBlock(
                byteDecrypt, 0, byteDecrypt.Length);

            _crypto.Dispose();

            return Encoding.UTF8.GetString(decrypted);
        }


    }
}
