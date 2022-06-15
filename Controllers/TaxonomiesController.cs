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
//    limitations under the License.using System.Collections.Generic;
using System.Collections.Generic;
using Dev10x.AspnetCore.Commons.Api.Exceptions;
using Dev10x.BasicTaxonomy.Dtos;
using Dev10x.BasicTaxonomy.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Dev10x.BasicTaxonomy.Controllers
{

    /// <summary>
    /// Rest Controller for Traxonomies
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TaxonomiesController : ControllerBase
    {
        //api services
        private readonly ITaxonomyService _taxonomyService;

        public TaxonomiesController(ITaxonomyService taxonomyService)
        {
            _taxonomyService = taxonomyService;
        }

        /// <summary>
        /// Return all taxonomy data or 404 error if dont have any
        /// </summary>
        /// <returns>List<TaxonomyDto></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public ActionResult<List<TaxonomyDto>> Get()
        {
            return Ok(_taxonomyService.FindAll());
        }
    }
}

