//
//  Copyright 2022  Soluciones Modernas 10x
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using NodaTime;

/// <summary>
/// This class exists derived from the time representation problems that .net has
/// For more information:
/// https://blog.nodatime.org/2011/08/what-wrong-with-datetime-anyway.html
/// 
/// </summary>
namespace Dev10x.AspnetCore.Utils.Date
{
    public class DateUtil
    {
        private readonly DateTimeZone _timeZone;

        //this class can only be instantiated by explicitly indicating the timezone
        public DateUtil(string timeZone)
        {
            _timeZone = DateTimeZoneProviders.Tzdb[timeZone];
        }

        //set to private to prevent it from being instantiated without the timezone
        public DateUtil()
        {
            _timeZone = DateTimeZoneProviders.Tzdb["America/Guatemala"];
        }


        /// <summary>
        /// Gets the current date and time, considering the timezone with which the class was instantiated
        /// </summary>
        /// <returns></returns>
        public DateTime GetTime()
        {
            return Instant.FromDateTimeUtc(DateTime.UtcNow)
                    .InZone(_timeZone)
                    .ToDateTimeUnspecified();
        }


    }
}