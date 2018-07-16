using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCantiere.PreventiviInCantiere.Models.HomeViewModels
{
    public class CantiereViewModel
    {
        public string Customer { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
