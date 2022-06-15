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
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dev10x.AspnetCore.Commons.Api.Exceptions;
using Dev10x.AspnetCore.Utils.Collections;
using Dev10x.BasicTaxonomy.Dtos;
using Dev10x.BasicTaxonomy.Helpers;
using Dev10x.BasicTaxonomy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dev10x.BasicTaxonomy.Services
{
    public class GenusService
    {
        private readonly DbService _dbService;
        private readonly FamilyService _familyService;
        private readonly IMapper _mapper;
        private ILogger<GenusService> _logger { get; }


        public GenusService(DbService dbService
            , FamilyService familyService
            , IMapper mapper
            , ILogger<GenusService> logger)
        {
            _dbService = dbService;
            _familyService = familyService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Return all rows of Family
        /// </summary>
        /// <returns> List<TaxonomyDto> </returns>
        /// <exception cref="ApiException"></exception>
        public List<GenusGet> FindAll()
        {
            List<Genus> result = _dbService.Genus
                                                   .AsNoTracking()
                                                   .ToList();

            return CollectionUtil.IsEmpty(result)
                ? throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_404)
                : _mapper.Map<List<GenusGet>>(result);

        }

        /// <summary>
        /// Find by criteria (id or name)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>List<Family></returns>
        /// <exception cref="ApiException"></exception>
        public List<Genus> Find(GenusGet criteria, bool exceptionOnNotExist = true)
        {
            IQueryable<Genus> result = _dbService.Genus
                                                  .AsNoTracking();
            //find by Family Id
            if (criteria.FamilyId > 0) { result = result.Where(r => r.FamilyId == criteria.FamilyId); }
            //find by Genus Id
            if (criteria.GenusId > 0) { result = result.Where(r => r.GenusId == criteria.GenusId); }

            //find by name 
            if (!string.IsNullOrWhiteSpace(criteria.GenusName)) { result = result.Where(r => r.GenusName.Equals(criteria.GenusName)); }

            return exceptionOnNotExist && CollectionUtil.IsEmpty(result)
                ? throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_404)
                : result.ToList();
        }

        /// <summary>
        /// Save a new genus
        /// </summary>
        /// <param name="genus"></param>
        public void Save(GenusPost genus)
        {
            //There must be a family to update
            _ = _familyService.Find(new FamilyDto { FamilyId = genus.FamilyId }).FirstOrDefault();

            //There must not be a genus with the same name
            if (!CollectionUtil.IsEmpty(Find(new GenusGet { GenusName = genus.GenusName }, false)))
                throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_422_EXIST);

            //generate the primary key
            int id = genus.FamilyId + GetNextSequenceValue();
            _logger.LogInformation("La llave generada es: " + id);

            //save  
            Genus model = new() { FamilyId = genus.FamilyId, GenusId = id, GenusName = genus.GenusName };
            _dbService.Add(model);
            _dbService.SaveChanges();
        }

        public int GetNextSequenceValue()
        {
            int nextVal = _dbService.Seqs
                .FromSqlRaw("select nextval('taxonomy.genus_seq') as sequence")
                .FirstOrDefault().Sequence;
            _logger.LogInformation("El nuevo valor de la secuencia es: " + nextVal);

            return nextVal;
        }
    }
}

