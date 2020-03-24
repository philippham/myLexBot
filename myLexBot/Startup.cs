// <copyright file="Startup.cs" company="pnphi49@gmail.com">
// Copyright (c) pnphi49@gmail.com. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using myLexBot.Options;
using myLexBot.Services;
using Amazon.Lex;
using Amazon.CognitoIdentity;
using Amazon;
using Microsoft.OpenApi.Models;

namespace myLexBot
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
            //Logger
            services.AddLogging();

            services.AddOptions();
            // Options
            services.Configure<LexOptions>(Configuration.GetSection("AWSConfiguration"));

            // AWS
            var cognitoPoolID = Configuration["AWSConfiguration:CognitoPoolID"];
            var region = Configuration["AWSConfiguration:BotRegion"];
            var svcRegionEndpoint = RegionEndpoint.GetBySystemName(region);
            var awsCredentials = new CognitoAWSCredentials(cognitoPoolID, svcRegionEndpoint);
            services.AddSingleton(new AmazonLexClient(awsCredentials, svcRegionEndpoint));

            // Services
            services.AddSingleton<IAWSLexService, AWSLexService>();

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My LexBot API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UsePathBase("/lexbotservice");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/lexbotservice/swagger/v1/swagger.json", "My Lex Bot V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
