using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB.ChildModels
{
    public partial class SurveyFormDetail
    {
        [Column("svId")]
        public int SvId { get; set; }
        [Key]
        [Column("houseId")]
        public int HouseId { get; set; }
        [StringLength(250)]
        public string ReferanceId { get; set; }
        [Column("houseLat")]
        [StringLength(500)]
        public string HouseLat { get; set; }
        [Column("houseLong")]
        [StringLength(550)]
        public string HouseLong { get; set; }
        [Column("name")]
        [StringLength(500)]
        public string Name { get; set; }
        [Column("mobileNumber")]
        [StringLength(250)]
        public string MobileNumber { get; set; }
        [Column("age")]
        public int? Age { get; set; }
        [Column("dateOfBirth", TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }
        [Column("gender")]
        [StringLength(50)]
        public string Gender { get; set; }
        [Column("bloodGroup")]
        [StringLength(50)]
        public string BloodGroup { get; set; }
        [Column("qualification")]
        [StringLength(50)]
        public string Qualification { get; set; }
        [Column("occupation")]
        [StringLength(50)]
        public string Occupation { get; set; }
        [Column("maritalStatus")]
        [StringLength(50)]
        public string MaritalStatus { get; set; }
        [Column("marriageDate", TypeName = "date")]
        public DateTime? MarriageDate { get; set; }
        [Column("livingStatus")]
        [StringLength(50)]
        public string LivingStatus { get; set; }
        [Column("totalMember")]
        public int? TotalMember { get; set; }
        [Column("willingStart")]
        public bool? WillingStart { get; set; }
        [Column("memberJobOtherCity")]
        public bool? MemberJobOtherCity { get; set; }
        [Column("noOfVehicle")]
        public int? NoOfVehicle { get; set; }
        [Column("vehicleType")]
        [StringLength(250)]
        public string VehicleType { get; set; }
        [Column("twoWheelerQty")]
        public int? TwoWheelerQty { get; set; }
        [Column("threeWheelerQty")]
        public int? ThreeWheelerQty { get; set; }
        [Column("fourWheelerQty")]
        public int? FourWheelerQty { get; set; }
        [Column("noPeopleVote")]
        public int? NoPeopleVote { get; set; }
        [Column("socialMedia")]
        [StringLength(250)]
        public string SocialMedia { get; set; }
        [Column("onlineShopping")]
        [StringLength(250)]
        public string OnlineShopping { get; set; }
        [Column("paymentModePrefer")]
        [StringLength(250)]
        public string PaymentModePrefer { get; set; }
        [Column("onlinePayApp")]
        [StringLength(250)]
        public string OnlinePayApp { get; set; }
        [Column("insurance")]
        [StringLength(250)]
        public string Insurance { get; set; }
        [Column("underInsurer")]
        public bool? UnderInsurer { get; set; }
        [Column("ayushmanBeneficiary")]
        public bool? AyushmanBeneficiary { get; set; }
        [Column("boosterShot")]
        public bool? BoosterShot { get; set; }
        [Column("memberDivyang")]
        public bool? MemberDivyang { get; set; }
        [Column("createUserId")]
        public int? CreateUserId { get; set; }
        [Column("createDate", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column("updateUserId")]
        public int? UpdateUserId { get; set; }
        [Column("updateDate", TypeName = "datetime")]
        public DateTime? UpdateDate { get; set; }
        [Column("resourcesAvailable")]
        [StringLength(250)]
        public string ResourcesAvailable { get; set; }
        [Column("totalAdults")]
        public int? TotalAdults { get; set; }
        [Column("totalChildren")]
        public int? TotalChildren { get; set; }
        [Column("totalSrCitizen")]
        public int? TotalSrCitizen { get; set; }
    }
}
