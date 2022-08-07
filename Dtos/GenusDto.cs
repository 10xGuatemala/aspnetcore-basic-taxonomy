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
    /// Genus class for post controllers
    /// </summary>
    public class GenusPost
    {
        /// <summary>
        /// Family Identifier - required for post (create) operations
        /// </summary>
        [Range(1, 999999, ErrorMessage = Constants.ERROR_LEN)]
        public int FamilyId { get; set; }

        /// <summary>
        /// Genus Name - required for post (create)  operations
        /// </summary>
        [StringLength(50, MinimumLength = 3, ErrorMessage = Constants.ERROR_LEN)]
        public string GenusName { get; set; }
    }
    /// <summary>
    /// Genus class for get controllers
    /// </summary>
    public class GenusGet
    {
        /// <summary>
        /// Family Identifier for get (search) operations
        /// </summary>
        public int FamilyId { get; set; }

        /// <summary>
        /// Genus Identifier for get (search) operations
        /// </summary>
        public int GenusId { get; set; }

        /// <summary>
        /// Genus Name for get (search) operations
        /// </summary>
        public string GenusName { get; set; }

    }



}

