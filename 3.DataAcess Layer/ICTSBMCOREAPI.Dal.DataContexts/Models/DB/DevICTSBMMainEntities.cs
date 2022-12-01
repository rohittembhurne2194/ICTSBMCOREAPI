﻿using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainModels;
using ICTSBMCOREAPI.Dal.DataContexts.Models.DB.MainSPModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICTSBMCOREAPI.Dal.DataContexts.Models.DB
{
    public class DevICTSBMMainEntities : DevSwachhBharatMainEntities
    {
        public DevICTSBMMainEntities()
        {

        }
        public DevICTSBMMainEntities(DbContextOptions<DevSwachhBharatMainEntities> options)
            : base(options)
        {
        }
        public DbSet<SP_UserLatLongDetailMain_Result> SP_UserLatLongDetailMain_Results { get; set; }
        public DbSet<SP_DistanceCount_Main_Result> SP_DistanceCount_Main_Results { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SP_UserLatLongDetailMain_Result>().HasNoKey();
            modelBuilder.Entity<SP_DistanceCount_Main_Result>().HasNoKey();
        }
    }
}
