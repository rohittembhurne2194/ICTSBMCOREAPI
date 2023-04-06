using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildSPModels
{
   public  class GetEmployeeWorkHistoryDetails_Result
    {
   
        public int gcType { get; set; }
        public string vehicleNumber { get; set; }
        public string Area { get; set; }
        public string ReferanceId { get; set; }
        public string Name { get; set; }
        public DateTime? gcDate { get; set; }
     
        
    }
}
