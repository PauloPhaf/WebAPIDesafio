using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebAPIProject
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
            services.AddCors(options =>
                {
                    options.AddPolicy("AllowAllOrigins",
                        builder => builder.AllowAnyOrigin());
                    options.AddPolicy("AllowAllMethods",
                        builder => builder.AllowAnyMethod());
                    options.AddPolicy("AllowAllHeaders",
                        builder => builder.AllowAnyHeader());
                    options.AddPolicy("AllowCredentials",
                        builder => builder.AllowCredentials());

                    //options.AddPolicy("AllowSpecificOrigin",
                    //    builder => builder.WithOrigins("http://localhost:4200"));
                }
            );

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");
            app.UseCors("AllowAllMethods");
            app.UseCors("AllowAllHeaders");
            app.UseCors("AllowCredentials");

            app.UseMvc();
        }
    }
}
