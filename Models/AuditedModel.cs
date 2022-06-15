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
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dev10x.BasicTaxonomy.Models
{
    /// <summary>
    /// Tables that need auditing must inherit from this class.
    /// Audit data is automatically added on save or update when models inherit from AuditedModel
    /// </summary>

    [Table("audit", Schema = "taxonomy")]
    public abstract class AuditedModel
    {
        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_on")]
        public DateTime CreatedOn { get; set; }

        [Column("modified_by")]
        public string ModifiedBy { get; set; }

        [Column("modified_on")]
        public DateTime? ModifiedOn { get; set; }

    }
}

