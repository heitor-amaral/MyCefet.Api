using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using MyCefet.Api.Services.Sigaa;
using MyCefet.Api.Services.Sigaa.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace MyCefet.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IInfoService, InfoService>();
            services.AddSingleton<IReportGradesService, ReportGradesService>();
            services.AddSingleton<IVirtualClassService, VirtualClassService>();
            services.AddSingleton<Services.Sinapse.Interfaces.ILoginService, Services.Sinapse.LoginService>();
            services.AddSingleton<Services.Sinapse.Interfaces.ILunchBalanceService, Services.Sinapse.LunchBalanceService>();

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Sigaa Scraper API",
                        Version = "v1",
                        Description = "",
                        Contact = new Contact
                        {
                            Name = "Heitor Amaral",
                            Url = "https://github.com/heitor-amaral"
                        }
                    });

                // Set the comments path for the Swagger JSON and UI.
               // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               // var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);
              //  c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
