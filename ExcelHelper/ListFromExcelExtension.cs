using Ganss.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHelper
{
    public static class ListFromExcelExtension
    {
        /// <summary>
        /// 从excel文件读取列表数据
        /// </summary>
        /// <typeparam name="T">类型 请声明到当前项目中(需要指定列名时)</typeparam>
        /// <param name="filePath">文件路径 支持 xlsx文件</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<T> FromExcel<T>(this string filePath)
        {
            if (Path.GetExtension(filePath) != ".xlsx")
            {
                throw new Exception("不支持的扩展名, 请使用xlsx文件");
            }
            return new ExcelMapper(filePath).Fetch<T>();

        }
        /// <summary>
        /// 从excel文件读取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IEnumerable<T> LoadFromExcel<T>(string filePath)
        {
            if (Path.GetExtension(filePath) != ".xlsx")
            {
                throw new Exception("不支持的扩展名, 请使用xlsx文件");
            }
            return new ExcelMapper(filePath).Fetch<T>();
        }

        public static IEnumerable<T> ReadExcelToList<T>(this IFormFile file)
        {
            using (var writer = new MemoryStream())
            {
                file.CopyTo(writer);

                return new ExcelMapper(writer).Fetch<T>();
            }
        }

       




    }
}
