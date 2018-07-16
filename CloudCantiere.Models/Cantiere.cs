using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.Models
{
    public class Cantiere
    {
        public string Customer { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
    }
}
