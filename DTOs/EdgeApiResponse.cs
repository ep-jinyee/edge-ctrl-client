using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EdgeCtrlClient.DTOs
{
    public class EdgeApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }

        public EdgeApiResponse(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
