﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class UserInApp
    {
        public int UserInAppsID { get; set; }
        public string UserId { get; set; }
        public int AppId { get; set; }
        public int subscriptionId { get; set; }
    }
}
