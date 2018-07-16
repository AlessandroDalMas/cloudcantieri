using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.Models
{
    public class Intervention
    {
        public int IdType { get; set; }
        public int IdCantiere { get; set; }
        public string Notes { get; set; }
        public double Price { get; set; }
    }
}
