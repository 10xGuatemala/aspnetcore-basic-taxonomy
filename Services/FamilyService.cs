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

namespace Dev10x.BasicTaxonomy.Services
{
    public class FamilyService
    {
        private readonly DbService _dbService;
        private readonly IMapper _mapper;

        public FamilyService(DbService dbService
            , IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all rows of Family
        /// </summary>
        /// <returns> List<TaxonomyDto> </returns>
        /// <exception cref="ApiException"></exception>
        public List<FamilyDto> FindAll()
        {
            List<Family> result = _dbService.Families
                                                   .AsNoTracking()
                                                   .ToList();

            return CollectionUtil.IsEmpty(result)
                ? throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_404)
                : _mapper.Map<List<FamilyDto>>(result);

        }

        /// <summary>
        /// Find by criteria (id or name)
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>List<Family></returns>
        /// <exception cref="ApiException"></exception>
        public List<Family> Find(FamilyDto criteria, bool exceptionOnNotExist = true)
        {
            IQueryable<Family> result = _dbService.Families
                                                  .AsNoTracking();
            //find by id
            if (criteria.FamilyId > 0) { result = result.Where(r => r.FamilyId == criteria.FamilyId); }
            //find by name 
            if (!string.IsNullOrWhiteSpace(criteria.FamilyName)) { result = result.Where(r => r.FamilyName.Equals(criteria.FamilyName)); }

            return exceptionOnNotExist && CollectionUtil.IsEmpty(result)
                ? throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_404)
                : result.ToList();
        }

        /// <summary>
        /// Save a Family new row
        /// </summary>
        /// <param name="newRecord"></param>
        /// <exception cref="ApiException"></exception>
        public void Save(string familyName)
        {
            //There must not be a family with the same name
            if (!CollectionUtil.IsEmpty(Find(new FamilyDto { FamilyName = familyName }, false)))
                throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_422_EXIST);

            Family model = new() { FamilyName = familyName };
            _dbService.Add(model);
            _dbService.SaveChanges();

        }

        /// <summary>
        /// Update a Family existing row
        /// </summary>
        /// <param name="newRecord"></param>
        /// <exception cref="ApiException"></exception>
        public void Update(FamilyDto updatedRow)
        {
            //There must be a family to update
            Family result = Find(new FamilyDto { FamilyId = updatedRow.FamilyId }).FirstOrDefault();
            result.FamilyName = updatedRow.FamilyName;

            _dbService.Families.Update(result);
            _dbService.SaveChanges();

        }

    }
}


