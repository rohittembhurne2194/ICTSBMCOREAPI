using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICTSBMCOREAPI.SwachhBhart.API.Bll.ViewModels.Models
{
   public  class SurveyFormDetails
    {
        public int houseId { get; set; }
        public string ReferanceId { get; set; }
        public string houseLat { get; set; }
        public string houseLong { get; set; }
        public string name { get; set; }
        public string mobileNumber { get; set; }
        public int age { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string bloodGroup { get; set; }
        public string qualification { get; set; }
        public string occupation { get; set; }
        public string maritalStatus { get; set; }
        public string marriageDate { get; set; }
        public string livingStatus { get; set; }
        public int totalMember { get; set; }
        public Nullable<bool> willingStart { get; set; }
        public Nullable<bool> memberJobOtherCity { get; set; }
        public int noOfVehicle { get; set; }
        public string vehicleType { get; set; }
        public int twoWheelerQty { get; set; }
        public int threeWheelerQty { get; set; }
        public int fourWheelerQty { get; set; }
        public int noPeopleVote { get; set; }
        public string socialMedia { get; set; }
        public string onlineShopping { get; set; }
        public string paymentModePrefer { get; set; }
        public string onlinePayApp { get; set; }
        public string insurance { get; set; }
        public Nullable<bool> underInsurer { get; set; }
        public Nullable<bool> ayushmanBeneficiary { get; set; }
        public Nullable<bool> boosterShot { get; set; }
        public Nullable<bool> memberDivyang { get; set; }
        public int createUserId { get; set; }
        public string createDate { get; set; }
        public int updateUserId { get; set; }
        public string updateDate { get; set; }

        public string resourcesAvailable { get; set; }
        public int totalAdults { get; set; }
        public int totalChildren { get; set; }
        public int totalSrCitizen { get; set; }
    }
}
