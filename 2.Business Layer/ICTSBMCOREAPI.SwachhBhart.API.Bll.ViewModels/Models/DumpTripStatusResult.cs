﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class DumpTripStatusResult
    {
        public int code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string errorMessages { get; set; }

        public string timestamp { get; set; }
        public dynamic data { get; set; }

        public string offlineId { get; set; }
    }
}
