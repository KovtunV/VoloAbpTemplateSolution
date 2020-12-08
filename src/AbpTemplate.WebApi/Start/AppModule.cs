using System;
using AbpTemplate.App;
using AbpTemplate.App.Services.Authorization;
using AbpTemplate.App.Synchronization;
using AbpTemplate.EF;
using AbpTemplate.EF.Automigration;
using AbpTemplate.EF.Bulk;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpTemplate.WebApi.Start
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(TemplateAppModule),
        typeof(SynchronizationModule),
        typeof(EFModule),
        typeof(EFBulkModule))]
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var services = context.Services;
            var configuration = services.GetConfiguration();

            ConfigureAuthentication(context, configuration);
            ConfigureSwaggerServices(services);
            ConfigureSendExceptionsDetailsToClients(services);
        }

        private void ConfigureSendExceptionsDetailsToClients(IServiceCollection services)
        {
            services.Configure<AbpExceptionHandlingOptions>(c => c.SendExceptionsDetailsToClients = true);
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            var jwtSecret = configuration.GetValue<string>("Settings:JwtSecret");

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,

                        ValidateLifetime = true,

                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(jwtSecret),
                        ValidateIssuerSigningKey = true,
                    };
                });
        }

        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Template API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);

                    // Add jwt auth to swagger
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Fill input: bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    var oaReference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    };

                    var securityScheme = new OpenApiSecurityScheme { Reference = oaReference };
                    var security = new OpenApiSecurityRequirement
                    {
                        { securityScheme, Array.Empty<string>() }
                    };
                    options.AddSecurityRequirement(security);

                    // Hide useless methods
                    options.DocumentFilter<SwaggerHideFilter>();
                }
            );
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Template API");
            });

            app.UseConfiguredEndpoints();

            MigrateDatabase(context);
        }

        private void MigrateDatabase(ApplicationInitializationContext context)
        {
            var cs = context.GetConfiguration().GetConnectionString("DefaultConnection");
            AutomigrationService.Migrate(cs);
        }
    }
}
