using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCantiere.PreventiviInCantiere.Models.HomeViewModels
{
    public class InterventionViewModel
    {
        public int IdType { get; set; }
        public int IdCantiere { get; set; }
        public string Notes { get; set; }
        public double Price { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
