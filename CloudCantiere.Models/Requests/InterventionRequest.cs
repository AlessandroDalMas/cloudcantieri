using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.Models.Requests
{
    public class InterventionRequest:Request
    {
        public InterventionRequest()
        {

        }
        public string Customer { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }

    }
}
