using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSFR_MainAPI.Data;

namespace SSFR_MainAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IDBRepository, DBRepository>();
           
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "SSFR Events API - Sergio Joel Ferreras", Version = "v1" });

            });

            services.AddMvc();

        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => 
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SSFR Events API");

            });

            app.UseMvc();
        }
    }
}
