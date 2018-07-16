using System;
using System.Collections.Generic;
using System.Text;

namespace CloudCantiere.Models.Requests
{
    public class InsertPhotoCantiereRequest : Request
    {
        public InsertPhotoCantiereRequest()
        {

        }
        public int IdCantiere { get; set; }
        public string URI { get; set; }
    }
}
