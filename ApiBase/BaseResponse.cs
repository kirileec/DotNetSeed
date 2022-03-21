using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ApiBase
{

    [KnownType(typeof(BaseResponse<string>))]
    public class BaseResponse<T>
    {
        
        public string code { get; set; }
        public long count { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }

}
