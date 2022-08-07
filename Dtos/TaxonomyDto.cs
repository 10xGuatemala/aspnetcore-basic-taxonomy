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
namespace Dev10x.BasicTaxonomy.Dtos
{
    /// <summary>
    /// Taxonomy Dto 
    /// </summary>
    public class TaxonomyDto
    {
        /// <summary>
        /// Family Identifier
        /// </summary>
        public int FamilyId { get; set; }
        /// <summary>
        /// Family Name
        /// </summary>
        public string FamilyName { get; set; }
        /// <summary>
        /// Genus Identifier (can be null)
        /// </summary>
        public int? GenusId { get; set; }
        /// <summary>
        /// Genus Name
        /// </summary>
        public string GenusName { get; set; }
        /// <summary>
        /// Specie Identifier (can be null)
        /// </summary>
        public int? SpecieId { get; set; }
        /// <summary>
        /// Specie Name
        /// </summary>
        public string SpecieName { get; set; }
    }
}

