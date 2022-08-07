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
using System.Text;

namespace Dev10x.BasicTaxonomy.Configurations
{
    /// <summary>
    /// Mapping representatio of JwtConfig section from application.json
    /// </summary>
    public class JwtConfig
    {
        /// <summary>
        /// JwtSecret from aplication.json
        /// </summary>
        public string JwtSecret { get; set; }

        /// <summary>
        /// JwtTokenExpiration from aplication.json
        /// </summary>
        public long JwtTokenExpiration { get; set; }

        /// <summary>
        /// TimeZone from aplication.json
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// key - string representation of secret from aplication.json
        /// </summary>
        private byte[] key;

        /// <summary>
        /// Get ascii encoding of jwtSecret
        /// </summary>
        /// <returns>byte representation</returns>
        public byte[] GetKey()
        {
            if (key == null)
            {
                key = Encoding.ASCII.GetBytes(JwtSecret);
            }
            return key;
        }

    }
}

