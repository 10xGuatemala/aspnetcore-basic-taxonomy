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
using System.ComponentModel.DataAnnotations;
using Dev10x.BasicTaxonomy.Helpers;

namespace Dev10x.BasicTaxonomy.Dtos
{
    /// <summary>
    /// Dto of family
    /// </summary>
    public class FamilyDto
    {
        /// <summary>
        /// Family Identifier
        /// </summary>
        public int FamilyId { get; set; }

        /// <summary>
        /// Family name - required 
        /// </summary>
        [StringLength(50, MinimumLength = 3, ErrorMessage = Constants.ERROR_LEN)]
        public string FamilyName { get; set; }


    }
}

