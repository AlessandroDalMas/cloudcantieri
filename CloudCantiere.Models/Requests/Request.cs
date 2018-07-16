using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.Models.Requests
{
    public abstract class Request
    {
        public Request()
        {
            RequestType = this.GetType().Name;
        }
        public string RequestType { get; set; }
    }
}
