using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExcelHelper
{
    public static class ListToExcelExtension
    {
        /// <summary>
        /// 将列表数据转为excel文件写出
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static bool ExportToXlsx<T>(this IEnumerable<T> list,string filePath,string sheetName = "Sheet1")
        {
            if (Path.GetExtension(filePath)!=".xlsx")
            {
                return false;
            }
            try
            {
                
                new Ganss.Excel.ExcelMapper().Save(filePath, list, sheetName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        /// <summary>
        /// 导出为字节数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static byte[] ExportToXlsx<T>(this IEnumerable<T> list,  string sheetName = "Sheet1")
        {

            byte[] result = null;
            using (var ms = new MemoryStream())
            {
                new Ganss.Excel.ExcelMapper().Save(ms, list, sheetName);
                result = ms.ToArray();
            }
            return result;
        }
        /// <summary>
        /// 导出到内存流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="ms"></param>
        /// <param name="sheetName"></param>
        public static void ExportToXlsx<T>(this IEnumerable<T> list,Stream ms ,string sheetName = "Sheet1")
        {
               new Ganss.Excel.ExcelMapper().Save(ms, list, sheetName);
        }


    }
}
