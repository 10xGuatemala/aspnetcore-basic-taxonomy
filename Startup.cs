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
using System.IO;
using System.Reflection;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Dev10x.AspnetCore.Commons.Api.Converters;
using Serilog;
using Dev10x.AspnetCore.Commons.Api.Exceptions;
using Dev10x.BasicTaxonomy.Helpers;
using Microsoft.AspNetCore.Http;
using Dev10x.BasicTaxonomy.Services;
using Microsoft.EntityFrameworkCore;
using Dev10x.BasicTaxonomy.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Dev10x.BasicTaxonomy.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Dev10x.AspnetCore.Utils.Date;

namespace Dev10x.BasicTaxonomy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // --------------------------- Basic API configurations --------------------------------
            services.AddProblemDetails(ConfigureProblemDetails)
                .AddControllers()
                .AddProblemDetailsConventions()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
                    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeOffsetConverter());
                });


            // --------------------------- Swagger ---------------------------------------------------
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Basic Taxonomy API",
                    Description = "This is a simple example for ASP.NET Core Web API",

                    Contact = new OpenApiContact
                    {
                        Name = "Soluciones Modernas 10x",
                        Email = "proyectos@10x.gt",
                        Url = new Uri("https://10x.gt"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under Apache Licence 2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // JWT swagger config
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization Header (Bearer).  Exemp: \"Authorization: Bearer {token}\""
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                          {
                          new OpenApiSecurityScheme
                            {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                            },
                            Array.Empty<string> ()

                        }
                });


            });

            // --------------------------- JWT -------------------------------------------
            IConfigurationSection apiSettingsSection = Configuration.GetSection("JwtConfig");
            services.Configure<JwtConfig>(apiSettingsSection);

            JwtConfig jwt = apiSettingsSection.Get<JwtConfig>();
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(x =>
               {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                   x.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(jwt.GetKey()),
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero
                   };
               });


            // ------------------------ Mapper --------------------------------
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // ------------------------ CORS ----------------------------------
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Content-Disposition"); // Header for files
            }));

            // ------------------------ API Services --------------------------
            services.AddSingleton<DateUtil>();


            services.AddHttpContextAccessor();
            services.AddTransient<IRequestService, RequestService>();
            services.AddScoped<ITaxonomyService, TaxonomyServiceOne>();
            services.AddScoped<FamilyService>();
            services.AddScoped<GenusService>();



            // ------------------------ Data Access ---------------------------
            services.AddDbContext<DbService>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("PostgresqlConnection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseCors("AllowAll");


                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasicTaxonomy v1"));
            }
            // handler exception
            app.UseProblemDetails();

            app.UseHttpsRedirection();

            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            // Only include exception details in a development environment
            options.IncludeExceptionDetails = (ctx, ex) =>
            {
                //WARN:Only returno true for debug
                return false;
            };
            //Api Customs Exceptions
            options.Map<ApiException>(exception => new ErrorDetails
            {
                Title = exception.Message,
                Status = exception.StatusCode,
                Date = new DateUtil().GetTime()

            });
            //"catch other exception"
            options.Map<Exception>(exception => new ErrorDetails
            {
                Title = Constants.ERROR_500,
                Status = StatusCodes.Status500InternalServerError,
                Date = new DateUtil().GetTime(),
                Detail = Constants.ERROR_500

            });


        }
    }

}
