using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ApiBase
{

    public class Response : BaseResponse<string, string> { }
    public class ResponseData<T> : BaseResponse<string, T> { }


    [KnownType(typeof(BaseResponse<string,string>))]
    public class BaseResponse<E,T>
    {
        
        public E code { get; set; }
        public long count { get; set; }
        public string msg { get; set; }
        public T data { get; set; }
    }

}
