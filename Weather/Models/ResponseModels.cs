using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Models
{
    public class ResponseModels
    {
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public int StatusCode { get; set; } = 200;
    }
}
