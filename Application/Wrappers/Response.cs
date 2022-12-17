using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, int code, string message = null)
        {
            Succeeded = true;
            Message = message;
            Code = code;
            Data = data;
        }
        public Response(string message)
        {
            Succeeded = false;
            Message = message;
        }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
