using System;
using System.Collections.Generic;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels
{
    public partial class AD_USER_MST_STREET
    {
        public int ADUM_USER_CODE { get; set; }
        public byte? SERVER_ID { get; set; }
        public int APP_ID { get; set; }
        public string ADUM_USER_ID { get; set; }
        public string ADUM_USER_NAME { get; set; }
        public string ADUM_LOGIN_ID { get; set; }
        public string ADUM_PASSWORD { get; set; }
        public string ADUM_EMPLOYEE_ID { get; set; }
        public string ADUM_DESIGNATION { get; set; }
        public string ADUM_MOBILE { get; set; }
        public string ADUM_EMAIL { get; set; }
        public string LOCAL_USER_NAME { get; set; }
        public string PROFILE_IMAGE { get; set; }
        public DateTime? ADUM_FRDT { get; set; }
        public DateTime? ADUM_TODT { get; set; }
        public bool? ADUM_STATUS { get; set; }
        public bool? UPDATE_FLAG { get; set; }
        public DateTime? LAST_UPDATE { get; set; }
        public int? AD_USER_TYPE_ID { get; set; }
        public string MOBILE_ID { get; set; }
        public bool? IS_ACTIVE { get; set; }
        public string IMO_NO { get; set; }
    }
}
