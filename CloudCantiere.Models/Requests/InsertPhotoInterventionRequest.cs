using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.Models.Requests
{
    public class InsertPhotoInterventionRequest : Request
    {
        public InsertPhotoInterventionRequest()
        {

        }

        public int IdIntervention { get; set; }
        public string URI { get; set; }

    }
}
