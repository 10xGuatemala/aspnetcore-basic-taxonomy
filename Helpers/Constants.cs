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

namespace Dev10x.BasicTaxonomy.Helpers
{
    /// <summary>
    /// Class to constants centralization
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant for error 500 description 
        /// </summary>
        public const string ERROR_500 = "Error desconocido, comuniquese con el administrador del sitio.";

        /// <summary>
        /// Constant for error 404 description 
        /// </summary>
        public const string ERROR_404 = "No se encontro la información solicitada";

        /// <summary>
        /// Constant for error 422 description 
        /// </summary>
        public const string ERROR_422_EXIST = "Ya existe un registro con la informacion que se quiere guardar";

        /// <summary>
        /// Constant for error description when required object
        /// </summary>
        public const string ERROR_REQUIRED = "El campo {0} es requerido.";

        /// <summary>
        /// Constant for description for length error
        /// </summary>
        public const string ERROR_LEN = "El campo {0} tiene un longitud invalida.";
    }
}

