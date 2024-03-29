﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class Result
    {
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string gismessage { get; set; }
        public string messageMar { get; set; }
        public string giserrorMessages { get; set; }
        public bool isAttendenceOn { get; set; }
        public bool isAttendenceOff { get; set; }

        public string emptype { get; set; }

        public string applink { get; set; }

        public int houseid { get; set; }
        public bool IsExixts { get; set; }
        public string referenceID { get; set; }
        public int dyId { get; set; }
        public string timestamp { get; set; }
        public int Id {get; set;}
    }
}
