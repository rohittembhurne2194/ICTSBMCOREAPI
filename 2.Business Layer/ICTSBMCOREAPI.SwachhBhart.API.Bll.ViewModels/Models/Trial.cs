﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
    public class Trial
    {
        public string id { get; set; }
        public int houseid { get; set; }
        public string startTs { get; set; }
        public string endTs { get; set; }
        public int createUser { get; set; }

        public int updateUser { get; set; }
        public string geom { get; set; }

        public string createTs { get; set; }
        public string updateTs { get; set; }

        public string? offlineId { get; set; }

        public int isRunning { get; set; }
    }
}
