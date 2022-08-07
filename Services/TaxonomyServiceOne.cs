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
using Dev10x.BasicTaxonomy.Interfaces;
using Dev10x.BasicTaxonomy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dev10x.BasicTaxonomy.Services
{
    /// <summary>
    /// Service for one interface implementation
    /// </summary>
    public class TaxonomyServiceOne : ITaxonomyService
    {
        private readonly DbService _dbService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for service injection
        /// </summary>
        /// <param name="dbService"></param>
        /// <param name="mapper"></param>
        public TaxonomyServiceOne(DbService dbService
            , IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all rows of DatamartView
        /// </summary>
        /// <returns> TaxonomyDto List </returns>
        /// <exception cref="ApiException"></exception>
        List<TaxonomyDto> ITaxonomyService.FindAll()
        {
            List<DatamartView> result = _dbService.Datamart
                                                        .AsNoTracking()
                                                        .ToList();

            return CollectionUtil.IsEmpty(result)
                ? throw new ApiException(StatusCodes.Status404NotFound, Constants.ERROR_404)
                : _mapper.Map<List<TaxonomyDto>>(result);

        }
    }
}

