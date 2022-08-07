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
using AutoMapper;
using Dev10x.BasicTaxonomy.Dtos;
using Dev10x.BasicTaxonomy.Models;

namespace Dev10x.BasicTaxonomy.Configurations
{
    /// <summary>
    /// Class for mapping a Dto to POCO and back
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// Default constructor with de all mapping available
        /// </summary>
        public AutoMapperConfig()
        {
            CreateMap<DatamartView, TaxonomyDto>();
            CreateMap<Family, FamilyDto>();
            CreateMap<FamilyDto, Family>();
            CreateMap<GenusGet, Genus>();
            CreateMap<Genus, GenusGet>();
        }
    }
}

