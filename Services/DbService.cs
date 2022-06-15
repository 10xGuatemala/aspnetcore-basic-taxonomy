//
//  Copyright 2022  Copyright Soluciones Modernas 10x
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System.Linq;
using Dev10x.AspnetCore.Utils.Date;
using Dev10x.BasicTaxonomy.Dtos;
using Dev10x.BasicTaxonomy.Interfaces;
using Dev10x.BasicTaxonomy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Dev10x.BasicTaxonomy.Services
{
    public class DbService : DbContext
    {

        private readonly DateUtil _myDateUtils;
        private readonly ILogger<DbService> _logger;
        private readonly IRequestService _requestService;


        public DbService(DbContextOptions<DbService> options
            , DateUtil myDateUtil
            , ILogger<DbService> logger
            , IRequestService requestService) : base(options)
        {

            _myDateUtils = myDateUtil;
            _logger = logger;
            _requestService = requestService;

        }

        // table mapping
        public DbSet<Family> Families { get; set; }
        public DbSet<Genus> Genus { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<DatamartView> Datamart { get; set; }

        //sequence
        public DbSet<IntSeq> Seqs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* API Fluent for composite keys*/

            modelBuilder.Entity<Genus>()
            .HasKey(c => new { c.FamilyId, c.GenusId });

            modelBuilder.Entity<Species>()
           .HasKey(c => new { c.FamilyId, c.GenusId, c.SpecieId });


        }


        /// <summary>
        /// Persist changes to database
        /// Overridden to support column auditing in tables that inherit from AuditedModel
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            OnSaveOrUpdate();
            return base.SaveChanges();
        }

        /// <summary>
        /// Adds the audit data to the tables that inherit from the AuditedModel When Saved or Updated
        /// </summary>
        private void OnSaveOrUpdate()
        {
            System.DateTime now = _myDateUtils.GetTime();
            _logger.LogInformation("La entidad es auditada, se registrara el cambio con fecha " + now);
            RequestUserDto requestUser = _requestService.GetRequestUser();
            _logger.LogInformation("La entidad es auditada, se registrara el cambio con usuario " + requestUser);


            //saved entity
            foreach (EntityEntry item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is AuditedModel))
            {
                AuditedModel entity = item.Entity as AuditedModel;
                entity.CreatedOn = now;
                entity.CreatedBy = requestUser.Username;
                entity.ModifiedOn = now;
                entity.ModifiedBy = requestUser.Username;
            }
            //updated entity
            foreach (EntityEntry item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified && e.Entity is AuditedModel))
            {
                AuditedModel entidad = item.Entity as AuditedModel;
                entidad.ModifiedOn = now;
                entidad.ModifiedBy = requestUser.Username;
                item.Property(nameof(entidad.CreatedOn)).IsModified = false;
                item.Property(nameof(entidad.CreatedBy)).IsModified = false;
            }
        }




    }
}

