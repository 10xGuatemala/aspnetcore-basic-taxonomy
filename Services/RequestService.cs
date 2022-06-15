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
using System.Security.Claims;
using Dev10x.BasicTaxonomy.Dtos;
using Dev10x.BasicTaxonomy.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Dev10x.BasicTaxonomy.Services
{
    public class RequestService : IRequestService
    {


        private readonly IHttpContextAccessor httpContextAccessor;
        private ILogger<RequestService> _logger { get; }

        public RequestService(IHttpContextAccessor httpContextAccessor
            , ILogger<RequestService> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            _logger = logger;


        }

        /// <summary>
        /// This method returns the data of the requesting user, including role.
        /// </summary>
        /// <returns>RequestUsuerDto</returns>
        public RequestUserDto GetRequestUser()
        {
            string currentUser = httpContextAccessor.HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(currentUser))
            {
                currentUser = "anonymous";
            }

            _logger.LogInformation("Request User: " + currentUser);

            return new RequestUserDto()
            {
                Username = currentUser,
                Role = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role)
            };

        }
    }
}

