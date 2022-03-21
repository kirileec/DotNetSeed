using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBase
{
    public static class ConvertExtension
    {
        public static T ConvertTo<T>(dynamic value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
